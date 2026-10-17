// =============================================
// TEXCEL ERP™ — Designation Module
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function getAntiForgeryToken() {
        var token = document.querySelector('#designationForm input[name="__RequestVerificationToken"]');
        if (!token) {
            token = document.querySelector('input[name="__RequestVerificationToken"]');
        }
        return token ? token.value : '';
    }

    function showToast(message, type) {
        var container = document.getElementById('toastContainer');
        if (!container) {
            alert(message);
            return;
        }

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

    // =============================================
    // MODAL CONTROLS
    // =============================================
    var modal = null;
    var modalTitle = null;
    var form = null;
    var submitBtnText = null;

    function openModal(mode, data) {
        if (!modal) return;

        var isEdit = mode === 'edit';

        if (modalTitle) {
            modalTitle.innerHTML = isEdit
                ? '<i class="fas fa-edit"></i> Edit Designation'
                : '<i class="fas fa-id-badge"></i> Add New Designation';
        }

        if (submitBtnText) {
            submitBtnText.textContent = isEdit ? 'Update Designation' : 'Create Designation';
        }

        if (form) {
            form.action = isEdit ? '/Designation/Edit' : '/Designation/Create';
        }

        if (isEdit && data) {
            setValue('DesignationId', data.DesignationId);
            setValue('DepartmentId', data.DepartmentId);
            setValue('DesignationCode', data.DesignationCode);
            setValue('DesignationName', data.DesignationName);
            setValue('Grade', data.Grade);
            setValue('Description', data.Description);
        } else {
            if (form) form.reset();
            setValue('DesignationId', 0);
        }

        modal.classList.add('show');
        document.body.style.overflow = 'hidden';

        var footer = document.querySelector('.footer');
        if (footer) footer.style.visibility = 'hidden';
    }

    function closeModal() {
        if (!modal) return;
        modal.classList.remove('show');
        document.body.style.overflow = '';

        var footer = document.querySelector('.footer');
        if (footer) footer.style.visibility = 'visible';
    }

    function setValue(name, value) {
        var el = form ? form.querySelector('[name="' + name + '"]') : null;
        if (el) el.value = value == null ? '' : value;
    }

    // =============================================
    // EDIT — Get designation and open modal
    // =============================================
    function handleEditClick(id) {
        fetch('/Designation/GetById/' + id, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    openModal('edit', d.data);
                } else {
                    showToast(d.message || 'Designation not found.', 'error');
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
        if (!confirm('Change designation status?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Designation/ToggleStatus', {
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
        if (!confirm('Delete this designation? This cannot be undone.')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/Designation/Delete', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'Designation deleted.', 'success');
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
        modal = document.getElementById('designationModal');
        modalTitle = document.getElementById('modalTitle');
        form = document.getElementById('designationForm');
        submitBtnText = document.getElementById('submitBtnText');

        var openBtn = document.getElementById('openModalBtn');
        var closeBtn = document.getElementById('closeModalBtn');
        var cancelBtn = document.getElementById('cancelBtn');

        if (openBtn) {
            openBtn.addEventListener('click', function () {
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

        // Delegated clicks — Delete → Toggle → Edit
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