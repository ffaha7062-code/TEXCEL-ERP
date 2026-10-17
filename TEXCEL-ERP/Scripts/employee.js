// =============================================
// TEXCEL ERP™ — Employee Module (AJAX)
// =============================================

(function () {
    'use strict';

    function getAntiForgeryToken() {
        var token = document.querySelector('#employeeForm input[name="__RequestVerificationToken"]');
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
        }, 4000);
    }

    var modal = null, modalTitle = null, form = null, saveBtn = null, submitBtnText = null;

    function fixModalHeight() {
        var box = document.querySelector('.modal-box');
        var body = document.querySelector('.modal-body');
        var top = document.querySelector('.modal-top');
        if (!box || !body || !top) return;

        var availableHeight = window.innerHeight - 130;
        box.style.height = availableHeight + 'px';
        box.style.maxHeight = availableHeight + 'px';
        box.style.display = 'flex';
        box.style.flexDirection = 'column';

        var topHeight = top.offsetHeight;
        var bodyHeight = availableHeight - topHeight;
        body.style.height = bodyHeight + 'px';
        body.style.maxHeight = bodyHeight + 'px';
        body.style.overflowY = 'auto';
        body.style.flex = 'none';
    }

    function setValue(name, value) {
        var el = form ? form.querySelector('[name="' + name + '"]') : null;
        if (el) el.value = value == null ? '' : value;
    }

    function resetForm() {
        if (!form) return;
        form.reset();
        setValue('EmployeeId', 0);
        var codeField = document.getElementById('EmployeeCode');
        if (codeField) codeField.value = 'Auto-generated';
        var jd = document.getElementById('JoiningDate');
        if (jd) jd.valueAsDate = new Date();
        var st = document.getElementById('Status');
        if (st) st.value = '3';
    }

    function openModal(mode, data) {
        if (!modal) return;
        var isEdit = mode === 'edit';

        if (modalTitle) {
            modalTitle.innerHTML = isEdit
                ? '<i class="fas fa-user-edit"></i> Edit Employee'
                : '<i class="fas fa-user-plus"></i> Add New Employee';
        }
        if (submitBtnText) submitBtnText.textContent = isEdit ? 'Update Employee' : 'Create Employee';

        if (isEdit && data) {
            setValue('EmployeeId', data.EmployeeId);
            setValue('EmployeeCode', data.EmployeeCode);
            setValue('BranchId', data.BranchId);
            setValue('DepartmentId', data.DepartmentId);
            setValue('DesignationId', data.DesignationId);
            setValue('ShiftId', data.ShiftId);
            setValue('FirstName', data.FirstName);
            setValue('LastName', data.LastName);
            setValue('CNIC', data.CNIC);
            setValue('DateOfBirth', data.DateOfBirth);
            setValue('Gender', data.Gender);
            setValue('MobileNumber', data.MobileNumber);
            setValue('Email', data.Email);
            setValue('CurrentAddress', data.CurrentAddress);
            setValue('JoiningDate', data.JoiningDate);
            setValue('Status', data.Status);
            setValue('SkillType', data.SkillType);
            setValue('MachineAssigned', data.MachineAssigned);
            setValue('BasicSalary', data.BasicSalary);
            setValue('PieceRate', data.PieceRate);
            setValue('BankName', data.BankName);
            setValue('BankAccountNumber', data.BankAccountNumber);
        } else {
            resetForm();
        }

        modal.classList.add('show');
        document.body.style.overflow = 'hidden';
        setTimeout(fixModalHeight, 100);

        var footer = document.querySelector('.footer');
        if (footer) footer.style.visibility = 'hidden';
    }

    function closeModal() {
        if (!modal) return;
        modal.classList.remove('show');
        document.body.style.overflow = '';
        var footer = document.querySelector('.footer');
        if (footer) footer.style.visibility = 'visible';

        var box = document.querySelector('.modal-box');
        var body = document.querySelector('.modal-body');
        if (box) { box.style.height = ''; box.style.maxHeight = ''; }
        if (body) { body.style.height = ''; body.style.maxHeight = ''; }
    }

    function handleEditClick(id) {
        fetch('/Employee/GetById/' + id, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) openModal('edit', d.data);
                else showToast(d.message || 'Employee not found.', 'error');
            })
            .catch(function (err) {
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    function handleFormSubmit(e) {
        e.preventDefault();

        var mode = form.getAttribute('data-mode') || 'create';
        var url = mode === 'edit' ? '/Employee/EditAjax' : '/Employee/CreateAjax';

        var formData = new FormData(form);

        if (saveBtn) {
            saveBtn.disabled = true;
            if (submitBtnText) submitBtnText.textContent = 'Saving...';
        }

        fetch(url, {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (response) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    if (submitBtnText) submitBtnText.textContent = mode === 'edit' ? 'Update Employee' : 'Create Employee';
                }

                if (response.success) {
                    showToast(response.message || 'Saved successfully!', 'success');
                    closeModal();
                    setTimeout(function () { location.reload(); }, 1200);   // 800 → 1200
                } else {
                    showToast(response.message || 'An error occurred.', 'error');
                }
            })
            .catch(function (err) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    if (submitBtnText) submitBtnText.textContent = mode === 'edit' ? 'Update Employee' : 'Create Employee';
                }
                showToast('Network error. Please try again.', 'error');
                console.error(err);
            });
    }

    function handleToggle(id) {
        if (!confirm('Change employee status?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Employee/ToggleStatusAjax', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'Status updated.', 'success');
                    setTimeout(function () { location.reload(); }, 800);
                } else {
                    showToast(d.message || 'Failed.', 'error');
                }
            })
            .catch(function (err) {
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    function handleDelete(id) {
        if (!confirm('Deactivate this employee? This cannot be undone.')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Employee/DeleteAjax', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'Employee deactivated.', 'success');
                    setTimeout(function () { location.reload(); }, 800);
                } else {
                    showToast(d.message || 'Failed.', 'error');
                }
            })
            .catch(function (err) {
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    function init() {
        modal = document.getElementById('employeeModal');
        modalTitle = document.getElementById('modalTitle');
        form = document.getElementById('employeeForm');
        saveBtn = document.getElementById('saveBtn');
        submitBtnText = document.getElementById('submitBtnText');

        var openBtn = document.getElementById('openModalBtn');
        var closeBtn = document.getElementById('closeModalBtn');
        var cancelBtn = document.getElementById('cancelBtn');

        if (openBtn) {
            openBtn.addEventListener('click', function () {
                form.setAttribute('data-mode', 'create');
                openModal('create');
            });
        }
        if (closeBtn) closeBtn.addEventListener('click', closeModal);
        if (cancelBtn) cancelBtn.addEventListener('click', closeModal);

        if (modal) {
            modal.addEventListener('click', function (e) {
                if (e.target === modal) closeModal();
            });
        }

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') closeModal();
        });

        if (form) {
            form.addEventListener('submit', handleFormSubmit);
        }

        document.addEventListener('click', function (e) {
            var delBtn = e.target.closest('.delete-btn');
            if (delBtn) {
                e.preventDefault();
                var did = delBtn.getAttribute('data-id');
                if (did) handleDelete(did);
                return;
            }

            var toggleBtn = e.target.closest('.toggle-btn');
            if (toggleBtn) {
                e.preventDefault();
                var tid = toggleBtn.getAttribute('data-id');
                if (tid) handleToggle(tid);
                return;
            }

            var editBtn = e.target.closest('.edit-btn');
            if (editBtn) {
                e.preventDefault();
                var eid = editBtn.getAttribute('data-id');
                if (eid) {
                    form.setAttribute('data-mode', 'edit');
                    handleEditClick(eid);
                }
                return;
            }
        });

        window.addEventListener('resize', function () {
            if (modal && modal.classList.contains('show')) fixModalHeight();
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();