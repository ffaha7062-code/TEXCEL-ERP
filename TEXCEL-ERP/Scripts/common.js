// =============================================
// TEXCEL ERP™ — Common Modal Helpers
// =============================================

window.TEXCEL = window.TEXCEL || {};

window.TEXCEL.hideFooter = function () {
    var footer = document.querySelector('.footer');
    if (footer) footer.style.visibility = 'hidden';
};

window.TEXCEL.showFooter = function () {
    var footer = document.querySelector('.footer');
    if (footer) footer.style.visibility = 'visible';
};

window.TEXCEL.openModal = function (modal) {
    if (modal) {
        modal.classList.add('show');
        document.body.style.overflow = 'hidden';
        window.TEXCEL.hideFooter();
    }
};

window.TEXCEL.closeModal = function (modal) {
    if (modal) {
        modal.classList.remove('show');
        document.body.style.overflow = '';
        window.TEXCEL.showFooter();
    }
};