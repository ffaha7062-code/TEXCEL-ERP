// =============================================
// TEXCEL ERP™ — Holiday Module
// Clean Version with Modal Blur Handling
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function getAntiForgeryToken() {
        var token = document.querySelector('#holidayForm input[name="__RequestVerificationToken"]');
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
        }, 3500);
    }

    var modal = null;
    var modalTitle = null;
    var form = null;
    var submitBtnText = null;

    // =============================================
    // MODAL OPEN
    // =============================================
    function openModal(mode, data) {
        if (!modal) return;
        var isEdit = mode === 'edit';

        if (modalTitle) {
            modalTitle.innerHTML = isEdit
                ? '<i class="fas fa-edit"></i> Edit Holiday'
                : '<i class="fas fa-calendar-plus"></i> Add Holiday';
        }
        if (submitBtnText) submitBtnText.textContent = isEdit ? 'Update Holiday' : 'Save Holiday';

        if (isEdit && data) {
            setValue('HolidayId', data.HolidayId);
            setValue('BranchId', data.BranchId);
            setValue('HolidayName', data.HolidayName);
            setValue('HolidayDate', data.HolidayDate);
            setValue('Description', data.Description);
            setCheckbox('IsPaid', data.IsPaid);
        } else {
            if (form) form.reset();
            setValue('HolidayId', 0);
            setCheckbox('IsPaid', true);
        }

        modal.classList.add('show');
        document.body.style.overflow = 'hidden';

        // ✅ Body pe class — CSS handle karega blur
        document.body.classList.add('modal-open');
    }

    // =============================================
    // MODAL CLOSE
    // =============================================
    function closeModal() {
        if (!modal) return;
        modal.classList.remove('show');
        document.body.style.overflow = '';
        document.body.classList.remove('modal-open');
    }

    function setValue(name, value) {
        var el = form ? form.querySelector('[name="' + name + '"]') : null;
        if (el) el.value = value == null ? '' : value;
    }

    function setCheckbox(name, value) {
        var el = form ? form.querySelector('[name="' + name + '"]') : null;
        if (el) el.checked = !!value;
    }

    // =============================================
    // FORM SUBMIT — AJAX
    // =============================================
    function handleFormSubmit(e) {
        e.preventDefault();

        var mode = form.getAttribute('data-mode') || 'create';
        var url = mode === 'edit' ? '/Holiday/Edit' : '/Holiday/Create';
        var saveBtn = document.getElementById('saveBtn');

        if (saveBtn) {
            saveBtn.disabled = true;
            if (submitBtnText) submitBtnText.textContent = 'Saving...';
        }

        var formData = new FormData(form);

        fetch(url, {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (response) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    if (submitBtnText) submitBtnText.textContent = mode === 'edit' ? 'Update Holiday' : 'Save Holiday';
                }

                if (response.success) {
                    showToast(response.message || 'Saved successfully!', 'success');
                    closeModal();
                    setTimeout(function () { location.reload(); }, 1000);
                } else {
                    showToast(response.message || 'Error occurred.', 'error');
                }
            })
            .catch(function (err) {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    if (submitBtnText) submitBtnText.textContent = mode === 'edit' ? 'Update Holiday' : 'Save Holiday';
                }
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    // =============================================
    // EDIT
    // =============================================
    function handleEditClick(id) {
        fetch('/Holiday/GetById/' + id, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    if (form) form.setAttribute('data-mode', 'edit');
                    openModal('edit', d.data);
                } else {
                    showToast(d.message || 'Holiday not found.', 'error');
                }
            })
            .catch(function (err) {
                showToast('Network error.', 'error');
                console.error(err);
            });
    }

    // =============================================
    // TOGGLE STATUS
    // =============================================
    function handleToggle(id) {
        if (!confirm('Change holiday status?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Holiday/ToggleStatus', {
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

    // =============================================
    // DELETE
    // =============================================
    function handleDelete(id) {
        if (!confirm('Delete this holiday?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Holiday/Delete', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'Holiday deleted.', 'success');
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

    // =============================================
    // INIT
    // =============================================
    function init() {
        modal = document.getElementById('holidayModal');
        modalTitle = document.getElementById('modalTitle');
        form = document.getElementById('holidayForm');
        submitBtnText = document.getElementById('submitBtnText');

        var openBtn = document.getElementById('openModalBtn');
        var closeBtn = document.getElementById('closeModalBtn');
        var cancelBtn = document.getElementById('cancelBtn');

        if (openBtn) {
            openBtn.addEventListener('click', function () {
                if (form) form.setAttribute('data-mode', 'create');
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

        // Delegated clicks
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
                if (eid) handleEditClick(eid);
                return;
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();