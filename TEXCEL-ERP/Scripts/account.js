// =============================================
// TEXCEL ERP™ — Account Module JavaScript
// =============================================

$(document).ready(function () {

    // =============================================
    // PASSWORD TOGGLE VISIBILITY
    // =============================================
    $(document).on('click', '.toggle-password', function () {
        var targetId = $(this).data('target') || 'passwordInput';
        var $input = $('#' + targetId);
        var $icon = $(this).find('i');

        if ($input.attr('type') === 'password') {
            $input.attr('type', 'text');
            $icon.removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            $input.attr('type', 'password');
            $icon.removeClass('fa-eye-slash').addClass('fa-eye');
        }
    });

    // =============================================
    // LOGIN FORM — AJAX SUBMIT
    // =============================================
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();

        var $form = $(this);
        var $btn = $('#loginBtn');
        var originalText = $btn.find('span').text();

        // Disable button + show loading
        $btn.prop('disabled', true).find('span').text('Signing in...');
        $btn.find('i').removeClass('fa-arrow-right-to-bracket').addClass('fa-spinner fa-spin');

        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            success: function (response) {
                // If server returns redirect, follow it
                if (response.redirectUrl) {
                    window.location.href = response.redirectUrl;
                } else {
                    // Reload to show errors
                    window.location.reload();
                }
            },
            error: function () {
                $btn.prop('disabled', false).find('span').text(originalText);
                $btn.find('i').removeClass('fa-spinner fa-spin').addClass('fa-arrow-right-to-bracket');
                alert('An error occurred. Please try again.');
            }
        });
    });

    // =============================================
    // REGISTER FORM — AJAX SUBMIT
    // =============================================
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

        var $form = $(this);
        var $btn = $form.find('button[type="submit"]');
        var originalText = $btn.find('span').text();

        $btn.prop('disabled', true).find('span').text('Creating account...');
        $btn.find('i').removeClass('fa-user-plus').addClass('fa-spinner fa-spin');

        $.ajax({
            url: $form.attr('action'),
            type: 'POST',
            data: $form.serialize(),
            success: function (response) {
                if (response.redirectUrl) {
                    window.location.href = response.redirectUrl;
                } else {
                    window.location.reload();
                }
            },
            error: function () {
                $btn.prop('disabled', false).find('span').text(originalText);
                $btn.find('i').removeClass('fa-spinner fa-spin').addClass('fa-user-plus');
                alert('An error occurred. Please try again.');
            }
        });
    });

    // =============================================
    // AUTO-FOCUS ON EMAIL FIELD
    // =============================================
    if ($('#Email').length && !$('#Email').val()) {
        $('#Email').focus();
    }

    // =============================================
    // PASSWORD STRENGTH INDICATOR (optional)
    // =============================================
    $('#regPassword, #newPassword').on('keyup', function () {
        var password = $(this).val();
        var strength = 0;

        if (password.length >= 6) strength++;
        if (password.length >= 10) strength++;
        if (/[A-Z]/.test(password)) strength++;
        if (/[0-9]/.test(password)) strength++;
        if (/[^A-Za-z0-9]/.test(password)) strength++;

        // You can add a visual indicator here if needed
    });

});