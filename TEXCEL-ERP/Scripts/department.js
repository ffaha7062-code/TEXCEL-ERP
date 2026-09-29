// =============================================
// TEXCEL ERP™ — Department Module
// =============================================

(function () {
    'use strict';

    function init() {
        var modal = document.getElementById('departmentModal');
        var modalTitle = document.getElementById('modalTitle');
        var openBtn = document.getElementById('openModalBtn');
        var closeBtn = document.getElementById('closeModalBtn');
        var cancelBtn = document.getElementById('cancelBtn');
        var form = document.getElementById('deptForm');
        var submitBtnText = document.getElementById('submitBtnText');

        console.log('Department JS initialized');
        console.log('Modal:', modal);
        console.log('Edit buttons:', document.querySelectorAll('.edit-btn').length);

        // =============================================
        // OPEN MODAL — ADD NEW
        // =============================================
        if (openBtn && modal) {
            openBtn.addEventListener('click', function () {
                if (form) form.reset();
                document.getElementById('DepartmentId').value = '0';
                document.getElementById('BranchId').value = '';

                if (modalTitle) modalTitle.innerHTML = '<i class="fas fa-sitemap"></i> Add New Department';
                if (submitBtnText) submitBtnText.textContent = 'Create Department';
                if (form) form.action = '/Department/Create';

                modal.classList.add('show');
                document.body.style.overflow = 'hidden';
            });
        }

        // =============================================
        // EDIT BUTTON — Event Delegation
        // =============================================
        document.addEventListener('click', function (e) {
            var editBtn = e.target.closest('.edit-btn');
            if (editBtn) {
                e.preventDefault();
                console.log('Edit button clicked');

                var id = editBtn.getAttribute('data-id');
                var code = editBtn.getAttribute('data-code');
                var name = editBtn.getAttribute('data-name');
                var desc = editBtn.getAttribute('data-desc');
                var branchId = editBtn.getAttribute('data-branch');

                console.log('Edit data:', { id: id, code: code, name: name, desc: desc, branch: branchId });

                // Fill form
                var idField = document.getElementById('DepartmentId');
                var codeField = document.getElementById('DepartmentCode');
                var nameField = document.getElementById('DepartmentName');
                var descField = document.getElementById('Description');
                var branchField = document.getElementById('BranchId');

                if (idField) idField.value = id;
                if (codeField) codeField.value = code || '';
                if (nameField) nameField.value = name || '';
                if (descField) descField.value = desc || '';
                if (branchField) branchField.value = branchId || '';

                // Title + Button text + Action
                if (modalTitle) modalTitle.innerHTML = '<i class="fas fa-edit"></i> Edit Department';
                if (submitBtnText) submitBtnText.textContent = 'Update Department';
                if (form) form.action = '/Department/Edit';

                // Open modal
                if (modal) {
                    modal.classList.add('show');
                    document.body.style.overflow = 'hidden';
                }
            }
        });

        // =============================================
        // CLOSE MODAL
        // =============================================
        function closeModal() {
            if (modal) {
                modal.classList.remove('show');
                document.body.style.overflow = '';
            }
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

        // =============================================
        // TOGGLE STATUS + DELETE
        // =============================================
        document.addEventListener('click', function (e) {
            var toggle = e.target.closest('.toggle-btn');
            if (toggle) {
                var id = toggle.getAttribute('data-id');
                if (!confirm('Change status?')) return;
                fetch('/Department/ToggleStatus/' + id, { method: 'POST' })
                    .then(function (r) { return r.json(); })
                    .then(function (d) {
                        if (d.success) location.reload();
                        else alert(d.message);
                    });
            }

            var del = e.target.closest('.delete-btn');
            if (del) {
                var did = del.getAttribute('data-id');
                if (!confirm('Delete this department?')) return;
                fetch('/Department/Delete/' + did, { method: 'POST' })
                    .then(function (r) { return r.json(); })
                    .then(function (d) {
                        if (d.success) location.reload();
                        else alert(d.message);
                    });
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();