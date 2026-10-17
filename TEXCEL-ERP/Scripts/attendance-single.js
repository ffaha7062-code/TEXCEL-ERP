// =============================================
// TEXCEL ERP™ — Single Attendance Entry
// =============================================

(function () {
    'use strict';

    function getAntiForgeryToken() {
        var token = document.querySelector('input[name="__RequestVerificationToken"]');
        return token ? token.value : '';
    }

    function showToast(message, type) {
        var container = document.getElementById('toastContainer');
        if (!container) { alert(message); return; }

        var toast = document.createElement('div');
        toast.className = 'toast toast-' + (type || 'info');
        var icon = type === 'success' ? 'fa-check-circle' : (type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle');
        toast.innerHTML = '<i class="fas ' + icon + '"></i><span>' + message + '</span>';
        container.appendChild(toast);

        setTimeout(function () {
            toast.style.opacity = '0';
            toast.style.transition = 'opacity 0.3s';
            setTimeout(function () { if (toast.parentNode) toast.parentNode.removeChild(toast); }, 300);
        }, 3500);
    }

    function parseTimeToMinutes(t) {
        if (!t) return null;
        var p = t.split(':');
        if (p.length < 2) return null;
        var h = parseInt(p[0], 10), m = parseInt(p[1], 10);
        if (isNaN(h) || isNaN(m)) return null;
        return h * 60 + m;
    }

    function minutesToHours(min) {
        return Math.round((min / 60) * 100) / 100;
    }

    function recalc() {
        var shiftData = document.getElementById('shiftData');
        if (!shiftData) return;

        var checkIn = document.getElementById('CheckIn').value;
        var checkOut = document.getElementById('CheckOut').value;
        var breakMin = parseInt(shiftData.getAttribute('data-break') || '0', 10);
        var shiftStart = shiftData.getAttribute('data-shift-start');
        var shiftEnd = shiftData.getAttribute('data-shift-end');

        var workedEl = document.getElementById('WorkedHours');
        var otEl = document.getElementById('OvertimeHours');
        var lessEl = document.getElementById('LessTimeHours');

        if (!workedEl || !otEl || !lessEl) return;

        workedEl.textContent = '0.00';
        otEl.textContent = '0.00';
        lessEl.textContent = '0.00';

        if (!checkIn || !checkOut) return;

        var inMin = parseTimeToMinutes(checkIn);
        var outMin = parseTimeToMinutes(checkOut);
        if (inMin === null || outMin === null) return;

        if (outMin < inMin) outMin += 24 * 60;

        var workedMin = outMin - inMin - breakMin;
        if (workedMin < 0) workedMin = 0;
        workedEl.textContent = minutesToHours(workedMin).toFixed(2);

        var shStart = parseTimeToMinutes(shiftStart);
        var shEnd = parseTimeToMinutes(shiftEnd);
        if (shStart === null || shEnd === null) return;

        if (shEnd < shStart) shEnd += 24 * 60;

        var shiftMin = shEnd - shStart - breakMin;

        if (workedMin > shiftMin) {
            otEl.textContent = minutesToHours(workedMin - shiftMin).toFixed(2);
        } else if (workedMin < shiftMin) {
            lessEl.textContent = minutesToHours(shiftMin - workedMin).toFixed(2);
        }
    }

    function handleSave() {
        var btn = document.getElementById('saveSingleBtn');
        if (!btn) return;

        var empId = parseInt(document.getElementById('EmployeeId').value, 10);
        var date = document.getElementById('AttendanceDate').value;
        var checkIn = document.getElementById('CheckIn').value;
        var checkOut = document.getElementById('CheckOut').value;
        var status = parseInt(document.getElementById('Status').value, 10);
        var isVerified = document.getElementById('IsVerified').checked;
        var remarks = document.getElementById('Remarks').value;

        if (!empId || !date) {
            showToast('Missing employee or date.', 'error');
            return;
        }

        btn.disabled = true;
        btn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving...';

        var fd = new FormData();
        fd.append('__RequestVerificationToken', getAntiForgeryToken());
        fd.append('employeeId', empId);
        fd.append('attendanceDate', date);
        fd.append('checkIn', checkIn || '');
        fd.append('checkOut', checkOut || '');
        fd.append('status', status);
        fd.append('isVerified', isVerified);
        fd.append('remarks', remarks || '');

        fetch('/Attendance/SaveSingle', {
            method: 'POST',
            body: fd,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-save"></i> Save Attendance';

                if (d.success) {
                    showToast(d.message || 'Saved successfully!', 'success');
                } else {
                    showToast(d.message || 'Error occurred.', 'error');
                }
            })
            .catch(function (err) {
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-save"></i> Save Attendance';
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    function init() {
        var card = document.getElementById('singleEntryCard');
        if (!card) return;

        var checkInEl = document.getElementById('CheckIn');
        var checkOutEl = document.getElementById('CheckOut');

        if (checkInEl) {
            checkInEl.addEventListener('change', recalc);
            checkInEl.addEventListener('input', recalc);
        }

        if (checkOutEl) {
            checkOutEl.addEventListener('change', recalc);
            checkOutEl.addEventListener('input', recalc);
        }

        var saveBtn = document.getElementById('saveSingleBtn');
        if (saveBtn) saveBtn.addEventListener('click', handleSave);

        recalc();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();