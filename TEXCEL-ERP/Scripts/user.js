// =============================================
// TEXCEL ERP™ — User Module
// Create/Edit via Modal, Toggle/Delete via AJAX
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function getAntiForgeryToken() {
        // ✅ Pehle userForm ka token, phir koi bhi
        var token = document.querySelector('#userForm input[name="__RequestVerificationToken"]');
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

    function openModal(mode, data) {
        if (!modal) return;

        var isEdit = mode === 'edit';

        if (modalTitle) {
            modalTitle.innerHTML = isEdit
                ? '<i class="fas fa-user-edit"></i> Edit User'
                : '<i class="fas fa-user-plus"></i> Add New User';
        }

        if (form) {
            form.action = isEdit ? '/User/Edit' : '/User/Create';
        }

        // Password field — Edit mein optional
        var pwdField = form ? form.querySelector('[name="Password"]') : null;
        var pwdLabel = pwdField ? pwdField.closest('.form-row') : null;
        if (pwdLabel) {
            pwdLabel.style.display = isEdit ? 'none' : '';
        }

        // Email field — Edit mein readonly
        var emailField = form ? form.querySelector('[name="Email"]') : null;
        if (emailField) {
            emailField.readOnly = isEdit;
            emailField.style.opacity = isEdit ? '0.7' : '';
        }

        if (isEdit && data) {
            setValue('Id', data.Id);
            setValue('FirstName', data.FirstName);
            setValue('LastName', data.LastName);
            setValue('Email', data.Email);
            setValue('PhoneNumber', data.PhoneNumber);
            setValue('BranchId', data.BranchId);
            setValue('RoleName', data.RoleName);
            setCheckbox('IsActive', data.IsActive);
        } else {
            if (form) form.reset();
            setValue('Id', '');
        }

        modal.classList.add('show');
        document.body.style.overflow = 'hidden';
    }

    function closeModal() {
        if (!modal) return;
        modal.classList.remove('show');
        document.body.style.overflow = '';
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
    // FORM SUBMIT — Create/Edit via AJAX
    // =============================================
    function handleFormSubmit(e) {
        e.preventDefault();

        var submitBtn = form.querySelector('button[type="submit"]');
        var originalText = submitBtn ? submitBtn.innerHTML : '';

        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving...';
        }

        var formData = new FormData(form);

        fetch(form.action, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (submitBtn) {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                }

                if (d.success) {
                    showToast(d.message || 'Saved successfully!', 'success');
                    closeModal();
                    setTimeout(function () { location.reload(); }, 800);
                } else {
                    showToast(d.message || 'An error occurred.', 'error');
                }
            })
            .catch(function (err) {
                if (submitBtn) {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                }
                showToast('Network error. Please try again.', 'error');
                console.error(err);
            });
    }

    // =============================================
    // EDIT BUTTON — Get user and open modal
    // =============================================
    function handleEditClick(id) {
        fetch('/User/GetById/' + id, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    openModal('edit', d.data);
                } else {
                    showToast(d.message || 'User not found.', 'error');
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
        if (!confirm('Change user status?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/User/ToggleStatus', {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
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
    // DELETE (Deactivate)
    // =============================================
    function handleDelete(id) {
        if (!confirm('Deactivate this user? This cannot be undone.')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);
        formData.append('id', id);

        fetch('/User/Delete', {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'User deactivated.', 'success');
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
        modal = document.getElementById('userModal');
        modalTitle = document.getElementById('modalTitle');
        form = document.getElementById('userForm');

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

        if (form) {
            form.addEventListener('submit', handleFormSubmit);
        }

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