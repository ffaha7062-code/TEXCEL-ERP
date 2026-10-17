// =============================================
// TEXCEL ERP™ — Branch Module
// Create/Edit via Modal, Toggle/Delete via AJAX
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function getAntiForgeryToken() {
        // ✅ Branch page ke form ka token use karein (logout ka nahi)
        var token = document.querySelector('#branchForm input[name="__RequestVerificationToken"]');
        if (!token) {
            // Fallback: koi bhi pehla token
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
                ? '<i class="fas fa-edit"></i> Edit Branch'
                : '<i class="fas fa-plus-circle"></i> Add New Branch';
        }

        if (form) {
            form.action = isEdit ? '/Branch/Edit' : '/Branch/Create';
        }

        if (isEdit && data) {
            setValue('BranchId', data.BranchId);
            setValue('CompanyId', data.CompanyId);
            setValue('BranchCode', data.BranchCode);
            setValue('BranchName', data.BranchName);
            setValue('City', data.City);
            setValue('Phone', data.Phone);
            setValue('Email', data.Email);
            setValue('ManagerName', data.ManagerName);
            setValue('Address', data.Address);
            setCheckbox('IsActive', data.IsActive);
        } else {
            if (form) form.reset();
            setValue('BranchId', 0);
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
    // EDIT BUTTON — Get branch and open modal
    // =============================================
    function handleEditClick(id) {
        console.log('=== EDIT CLICKED: id=' + id);

        fetch('/Branch/GetById/' + id, {
            method: 'GET',
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    openModal('edit', d.data);
                } else {
                    showToast(d.message || 'Branch not found.', 'error');
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
        console.log('=== TOGGLE CLICKED: id=' + id);   // ✅ DEBUG

        if (!confirm('Change branch status?')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);

        fetch('/Branch/ToggleStatus/' + id, {
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
    // DELETE
    // =============================================
    function handleDelete(id) {
        console.log('=== DELETE CLICKED: id=' + id);   // ✅ DEBUG

        if (!confirm('Delete this branch? This cannot be undone.')) return;

        var token = getAntiForgeryToken();
        var formData = new FormData();
        formData.append('__RequestVerificationToken', token);

        fetch('/Branch/Delete/' + id, {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
            .then(function (r) { return r.json(); })
            .then(function (d) {
                if (d.success) {
                    showToast(d.message || 'Branch deleted.', 'success');
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
        console.log('=== BRANCH.JS INIT ===');

        modal = document.getElementById('branchModal');
        modalTitle = document.getElementById('modalTitle');
        form = document.getElementById('branchForm');

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

        // =============================================
        // DELEGATED CLICKS — Edit / Toggle / Delete
        // ✅ IMPORTANT: Delete ko PEHLE check karein (specific → general)
        // =============================================
        document.addEventListener('click', function (e) {
            // Delete check PEHLE (kyunki "delete" class zyada specific hai)
            var delBtn = e.target.closest('.delete-btn');
            if (delBtn) {
                e.preventDefault();
                var did = delBtn.getAttribute('data-id');
                console.log('=== DELETE BTN DETECTED: id=' + did);
                if (did) handleDelete(did);
                return;
            }

            // Toggle
            var toggleBtn = e.target.closest('.toggle-btn');
            if (toggleBtn) {
                e.preventDefault();
                var tid = toggleBtn.getAttribute('data-id');
                console.log('=== TOGGLE BTN DETECTED: id=' + tid);
                if (tid) handleToggle(tid);
                return;
            }

            // Edit
            var editBtn = e.target.closest('.edit-btn');
            if (editBtn) {
                e.preventDefault();
                var eid = editBtn.getAttribute('data-id');
                console.log('=== EDIT BTN DETECTED: id=' + eid);
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