// =============================================
// TEXCEL ERP™ — Attendance Module
// Bulk Entry + OT/Less Auto-Calc + AJAX Save
// Smart Validation: Present/HalfDay = IN/OUT required
// Absent/Leave/Holiday/Weekend = IN/OUT optional
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function getAntiForgeryToken() {
        var token = document.querySelector('#bulkAttendanceForm input[name="__RequestVerificationToken"]');
        if (!token) token = document.querySelector('input[name="__RequestVerificationToken"]');
        return token ? token.value : '';
    }

    function showToast(message, type) {
        var container = document.getElementById('toastContainer');
        if (!container) { alert(message); return; }

        var toast = document.createElement('div');
        toast.className = 'toast toast-' + (type || 'info');

        var icon = 'fa-info-circle';
        if (type === 'success') icon = 'fa-check-circle';
        if (type === 'error') icon = 'fa-exclamation-circle';

        toast.innerHTML = '<i class="fas ' + icon + '"></i><span>' + message + '</span>';
        container.appendChild(toast);

        setTimeout(function () {
            toast.classList.add('toast-hide');
            setTimeout(function () {
                if (toast.parentNode) toast.parentNode.removeChild(toast);
            }, 300);
        }, 5000);   // 5 sec for error messages
    }

    // =============================================
    // TIME PARSING — HH:MM to Decimal Hours
    // =============================================
    function parseTimeToMinutes(timeStr) {
        if (!timeStr) return null;
        var parts = timeStr.split(':');
        if (parts.length < 2) return null;
        var h = parseInt(parts[0], 10);
        var m = parseInt(parts[1], 10);
        if (isNaN(h) || isNaN(m)) return null;
        return h * 60 + m;
    }

    function minutesToHours(min) {
        return Math.round((min / 60) * 100) / 100;
    }

    // =============================================
    // STATUS → TIME REQUIREMENT CHECK
    // 1=Present, 2=Absent, 3=Leave, 4=HalfDay, 5=Holiday, 6=Weekend
    // =============================================
    function requiresTime(status) {
        return (status === 1 || status === 4);   // Present, Half Day
    }

    // =============================================
    // AUTO-CALCULATE ROW
    // =============================================
    function calculateRow(tr) {
        var checkIn = tr.querySelector('.check-in').value;
        var checkOut = tr.querySelector('.check-out').value;
        var status = parseInt(tr.querySelector('.status-select').value || '2', 10);
        var breakMinutes = parseInt(tr.getAttribute('data-break') || '0', 10);

        var shiftStart = tr.getAttribute('data-shift-start');
        var shiftEnd = tr.getAttribute('data-shift-end');

        var workedEl = tr.querySelector('.worked-hours');
        var lessEl = tr.querySelector('.less-time');
        var otEl = tr.querySelector('.ot-time');
        var lateEl = tr.querySelector('.late-min');

        // Reset
        workedEl.textContent = '0.00';
        lessEl.textContent = '0.00';
        otEl.textContent = '0.00';
        lateEl.textContent = '0';

        // ✅ Agar status Absent/Leave/Holiday/Weekend hai — calculation nahi
        if (!requiresTime(status)) {
            return;
        }

        // ✅ Shift missing check
        var hasShift = shiftStart && shiftEnd &&
            shiftStart !== '00:00' && shiftEnd !== '00:00' &&
            shiftStart !== '' && shiftEnd !== '';

        if (!hasShift) {
            // Shift assign nahi hai — sirf Total Work calculate karein
            if (checkIn && checkOut) {
                var inMin = parseTimeToMinutes(checkIn);
                var outMin = parseTimeToMinutes(checkOut);
                if (inMin !== null && outMin !== null) {
                    if (outMin < inMin) outMin += 24 * 60;
                    var workedMinutes = outMin - inMin - breakMinutes;
                    if (workedMinutes < 0) workedMinutes = 0;
                    workedEl.textContent = minutesToHours(workedMinutes).toFixed(2);
                }
            }
            return;
        }

        // Sirf tab calculate karein jab CheckIn/CheckOut ho
        if (!checkIn || !checkOut) return;

        var inMin = parseTimeToMinutes(checkIn);
        var outMin = parseTimeToMinutes(checkOut);

        if (inMin === null || outMin === null) return;

        // Overnight shift handle
        if (outMin < inMin) {
            outMin += 24 * 60;
        }

        // Total worked minutes
        var workedMinutes = outMin - inMin - breakMinutes;
        if (workedMinutes < 0) workedMinutes = 0;
        workedEl.textContent = minutesToHours(workedMinutes).toFixed(2);

        // Shift duration minutes
        var shiftStartMin = parseTimeToMinutes(shiftStart);
        var shiftEndMin = parseTimeToMinutes(shiftEnd);

        if (shiftStartMin !== null && shiftEndMin !== null) {
            // Overnight shift
            if (shiftEndMin < shiftStartMin) {
                shiftEndMin += 24 * 60;
            }

            var shiftMinutes = shiftEndMin - shiftStartMin - breakMinutes;

            // OT / Less
            if (workedMinutes > shiftMinutes) {
                var otHours = minutesToHours(workedMinutes - shiftMinutes);
                otEl.textContent = otHours.toFixed(2);
                lessEl.textContent = '0.00';
            } else if (workedMinutes < shiftMinutes) {
                var lessHours = minutesToHours(shiftMinutes - workedMinutes);
                lessEl.textContent = lessHours.toFixed(2);
                otEl.textContent = '0.00';
            }

            // Late
            if (inMin > shiftStartMin) {
                var lateMin = inMin - shiftStartMin;
                lateEl.textContent = lateMin;
            }
        }
    }

    // =============================================
    // TOGGLE TIME FIELDS (Enable/Disable)
    // =============================================
    function toggleTimeFields(tr, status) {
        var checkIn = tr.querySelector('.check-in');
        var checkOut = tr.querySelector('.check-out');

        if (requiresTime(status)) {
            // Enable
            checkIn.disabled = false;
            checkOut.disabled = false;
            checkIn.style.opacity = '1';
            checkOut.style.opacity = '1';
        } else {
            // Disable + Clear
            checkIn.value = '';
            checkOut.value = '';
            checkIn.disabled = true;
            checkOut.disabled = true;
            checkIn.style.opacity = '0.4';
            checkOut.style.opacity = '0.4';
        }
    }

    // =============================================
    // BULK SAVE — AJAX (With Smart Validation)
    // =============================================
    function handleBulkSave(e) {
        e.preventDefault();

        var form = document.getElementById('bulkAttendanceForm');
        if (!form) return;

        var table = document.getElementById('attendanceTable');
        if (!table) {
            showToast('No attendance data to save.', 'error');
            return;
        }

        var rows = table.querySelectorAll('tbody tr');
        if (rows.length === 0) {
            showToast('No employees to save.', 'error');
            return;
        }

        var attendanceDate = form.querySelector('input[name="attendanceDate"]').value;
        var data = [];
        var errors = [];

        rows.forEach(function (tr) {
            var empId = parseInt(tr.getAttribute('data-employee-id'), 10);
            var empNameEl = tr.querySelector('.emp-name-cell');
            var empName = empNameEl ? empNameEl.textContent.trim() : 'Employee ' + empId;

            var checkIn = tr.querySelector('.check-in').value || '';
            var checkOut = tr.querySelector('.check-out').value || '';
            var status = parseInt(tr.querySelector('.status-select').value || '2', 10);
            var isVerified = tr.querySelector('.is-verified').checked;

            // ✅ SMART VALIDATION
            // Present (1) aur HalfDay (4) ke liye IN/OUT required
            if (requiresTime(status)) {
                if (!checkIn) {
                    errors.push(empName + ' → Check In required');
                    return;
                }
                if (!checkOut) {
                    errors.push(empName + ' → Check Out required');
                    return;
                }
            } else {
                // Absent/Leave/Holiday/Weekend — IN/OUT clear
                checkIn = '';
                checkOut = '';
            }

            data.push({
                EmployeeId: empId,
                CheckIn: checkIn,
                CheckOut: checkOut,
                Status: status,
                IsVerified: isVerified,
                Remarks: ''
            });
        });

        // ✅ Agar errors hain — show karein aur ruk jayein
        if (errors.length > 0) {
            var errorMsg = errors.slice(0, 5).join(' | ');
            if (errors.length > 5) {
                errorMsg += ' ... and ' + (errors.length - 5) + ' more';
            }
            showToast('Errors: ' + errorMsg, 'error');
            return;
        }

        var saveBtn = document.getElementById('saveAllBtn');
        if (saveBtn) {
            saveBtn.disabled = true;
            saveBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving...';
        }

        // Build form data
        var formData = new FormData();
        formData.append('__RequestVerificationToken', getAntiForgeryToken());
        formData.append('attendanceDate', attendanceDate);

        data.forEach(function (item, index) {
            formData.append('model[' + index + '].EmployeeId', item.EmployeeId);
            formData.append('model[' + index + '].CheckIn', item.CheckIn);
            formData.append('model[' + index + '].CheckOut', item.CheckOut);
            formData.append('model[' + index + '].Status', item.Status);
            formData.append('model[' + index + '].IsVerified', item.IsVerified);
            formData.append('model[' + index + '].Remarks', item.Remarks || '');
        });

        fetch('/Attendance/SaveBulk', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (response) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    saveBtn.innerHTML = '<i class="fas fa-save"></i> Save All Attendance';
                }

                if (response.success) {
                    showToast(response.message || 'Attendance saved successfully!', 'success');
                    setTimeout(function () { location.reload(); }, 1200);
                } else {
                    showToast(response.message || 'Failed to save.', 'error');
                }
            })
            .catch(function (err) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    saveBtn.innerHTML = '<i class="fas fa-save"></i> Save All Attendance';
                }
                showToast('Network error. Please try again.', 'error');
                console.error(err);
            });
    }

    // =============================================
    // BULK ACTIONS (Mark All Present/Absent/Clear)
    // =============================================
    function handleBulkAction(action) {
        var table = document.getElementById('attendanceTable');
        if (!table) return;

        var rows = table.querySelectorAll('tbody tr');

        rows.forEach(function (tr) {
            var statusSelect = tr.querySelector('.status-select');
            var checkIn = tr.querySelector('.check-in');
            var checkOut = tr.querySelector('.check-out');
            var shiftStart = tr.getAttribute('data-shift-start');
            var shiftEnd = tr.getAttribute('data-shift-end');

            if (action === 'present') {
                statusSelect.value = '1';
                checkIn.value = shiftStart || '';
                checkOut.value = shiftEnd || '';
                toggleTimeFields(tr, 1);
            } else if (action === 'absent') {
                statusSelect.value = '2';
                toggleTimeFields(tr, 2);
            } else if (action === 'clear') {
                statusSelect.value = '2';
                toggleTimeFields(tr, 2);
            }

            calculateRow(tr);
        });

        showToast('Bulk action applied. Click Save to persist.', 'info');
    }

    // =============================================
    // INIT
    // =============================================
    function init() {
        var form = document.getElementById('bulkAttendanceForm');
        var table = document.getElementById('attendanceTable');

        if (!form || !table) return;

        // Form submit
        form.addEventListener('submit', handleBulkSave);

        // Row-wise auto-calc + toggle time fields
        table.addEventListener('change', function (e) {
            var tr = e.target.closest('tr');
            if (!tr) return;

            // ✅ Status change → toggle time fields
            if (e.target.classList.contains('status-select')) {
                var status = parseInt(e.target.value, 10);
                toggleTimeFields(tr, status);
                calculateRow(tr);
                return;
            }

            // CheckIn/CheckOut change → recalculate
            if (e.target.classList.contains('check-in') ||
                e.target.classList.contains('check-out')) {
                calculateRow(tr);
            }
        });

        // Input event for real-time
        table.addEventListener('input', function (e) {
            var tr = e.target.closest('tr');
            if (!tr) return;

            if (e.target.classList.contains('check-in') ||
                e.target.classList.contains('check-out')) {
                calculateRow(tr);
            }
        });

        // Bulk action buttons
        var bulkPresentBtn = document.getElementById('bulkPresentBtn');
        var bulkAbsentBtn = document.getElementById('bulkAbsentBtn');
        var bulkClearBtn = document.getElementById('bulkClearBtn');

        if (bulkPresentBtn) {
            bulkPresentBtn.addEventListener('click', function () { handleBulkAction('present'); });
        }
        if (bulkAbsentBtn) {
            bulkAbsentBtn.addEventListener('click', function () { handleBulkAction('absent'); });
        }
        if (bulkClearBtn) {
            bulkClearBtn.addEventListener('click', function () { handleBulkAction('clear'); });
        }

        // ✅ Initial setup — har row ke liye time fields toggle karein
        var rows = table.querySelectorAll('tbody tr');
        rows.forEach(function (tr) {
            var statusSelect = tr.querySelector('.status-select');
            if (statusSelect) {
                var status = parseInt(statusSelect.value || '2', 10);
                toggleTimeFields(tr, status);
            }
            calculateRow(tr);
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();