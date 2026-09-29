// =============================================
// TEXCEL ERP™ — User Module
// =============================================

(function () {
    'use strict';

    function init() {
        var modal = document.getElementById('userModal');
        var openBtn = document.getElementById('openModalBtn');
        var closeBtn = document.getElementById('closeModalBtn');
        var cancelBtn = document.getElementById('cancelBtn');

        if (openBtn && modal) {
            openBtn.addEventListener('click', function () {
                modal.classList.add('show');
                document.body.style.overflow = 'hidden';

                // ✅ Footer ko hide karein
                var footer = document.querySelector('.footer');
                if (footer) footer.style.visibility = 'hidden';
            });
        }

        function closeModal() {
            if (modal) {
                modal.classList.remove('show');
                document.body.style.overflow = '';

                // ✅ Footer ko wapas show karein
                var footer = document.querySelector('.footer');
                if (footer) footer.style.visibility = 'visible';
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

        // Toggle Status
        document.addEventListener('click', function (e) {
            var toggle = e.target.closest('.toggle-btn');
            if (toggle) {
                var id = toggle.getAttribute('data-id');
                if (!confirm('Change status?')) return;
                fetch('/User/ToggleStatus?id=' + id, { method: 'POST' })
                    .then(function (r) { return r.json(); })
                    .then(function (d) {
                        if (d.success) location.reload();
                        else alert(d.message);
                    });
            }

            var del = e.target.closest('.delete-btn');
            if (del) {
                var did = del.getAttribute('data-id');
                if (!confirm('Deactivate this user?')) return;
                fetch('/User/Delete?id=' + did, { method: 'POST' })
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