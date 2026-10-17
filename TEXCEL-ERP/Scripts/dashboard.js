// =============================================
// TEXCEL ERP™ — Premium Dashboard JavaScript
// =============================================





   

(function () {
    'use strict';

    // ✅ Global chart instance — destroy ke liye
    var salesProductionChartInstance = null;

    document.addEventListener('DOMContentLoaded', function () {
        // =============================================
        // DROPDOWN MENU TOGGLE (Click for Mobile + Desktop)
        // =============================================
        document.addEventListener('click', function (e) {
            var hasDropdown = e.target.closest('.has-dropdown');
            var isMobile = window.innerWidth <= 992;

            // Desktop: hover works, click on dropdown link prevents default
            if (!isMobile && hasDropdown) {
                var link = e.target.closest('.nav-link');
                if (link && link.getAttribute('href') === 'javascript:void(0);') {
                    e.preventDefault();
                }
                return;
            }

            // Mobile: click toggle
            if (isMobile) {
                // Close all others
                document.querySelectorAll('.has-dropdown.open').forEach(function (el) {
                    if (el !== hasDropdown) el.classList.remove('open');
                });

                if (hasDropdown) {
                    var link = e.target.closest('.nav-link');
                    if (link && link.getAttribute('href') === 'javascript:void(0);') {
                        e.preventDefault();
                        hasDropdown.classList.toggle('open');
                    }
                } else {
                    document.querySelectorAll('.has-dropdown.open').forEach(function (el) {
                        el.classList.remove('open');
                    });
                }
            }
            // =============================================
            // USER PROFILE DROPDOWN TOGGLE
            // =============================================
            var userProfileBtn = document.getElementById('userProfileBtn');
            var userDropdown = document.getElementById('userDropdown');

            if (userProfileBtn && userDropdown) {
                userProfileBtn.addEventListener('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    // Close settings dropdown if open
                    if (settingsDropdown) settingsDropdown.classList.remove('open');
                    userDropdown.classList.toggle('open');
                });

                // Close on ESC
                document.addEventListener('keydown', function (e) {
                    if (e.key === 'Escape') {
                        userDropdown.classList.remove('open');
                    }
                });
            }
            // =============================================
            // SETTINGS DROPDOWN TOGGLE
            // =============================================
            var settingsBtn = document.getElementById('settingsBtn');
            var settingsDropdown = document.getElementById('settingsDropdown');

            if (settingsBtn && settingsDropdown) {
                settingsBtn.addEventListener('click', function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    // Close user dropdown if open
                    if (userDropdown) userDropdown.classList.remove('open');
                    settingsDropdown.classList.toggle('open');
                });

                // Close on outside click
                document.addEventListener('click', function (e) {
                    if (!settingsDropdown.contains(e.target) && !settingsBtn.contains(e.target)) {
                        settingsDropdown.classList.remove('open');
                    }
                });

                // Close on ESC
                document.addEventListener('keydown', function (e) {
                    if (e.key === 'Escape') {
                        settingsDropdown.classList.remove('open');
                    }
                });
            }
        });
        initTheme();
        initMobileMenu();
        initClock();
        initTaskToggle();
        initNotifications();
        initCharts();
    });

    // =============================================
    // THEME TOGGLE (Dark / Light)
    // =============================================
    function initTheme() {
        var savedTheme = localStorage.getItem('texcel-theme') || 'light';
        applyTheme(savedTheme);

        var toggleBtn = document.getElementById('themeToggle');
        if (toggleBtn) {
            toggleBtn.addEventListener('click', function () {
                var current = document.documentElement.getAttribute('data-theme') || 'light';
                var next = current === 'dark' ? 'light' : 'dark';
                applyTheme(next);
                localStorage.setItem('texcel-theme', next);
            });
        }
    }

    function applyTheme(theme) {
        var icon = document.getElementById('themeIcon');
        if (theme === 'dark') {
            document.documentElement.setAttribute('data-theme', 'dark');
            if (icon) {
                icon.classList.remove('fa-moon');
                icon.classList.add('fa-sun');
            }
        } else {
            document.documentElement.removeAttribute('data-theme');
            if (icon) {
                icon.classList.remove('fa-sun');
                icon.classList.add('fa-moon');
            }
        }
    }

    // =============================================
    // MOBILE MENU
    // =============================================
    function initMobileMenu() {
        var toggle = document.getElementById('mobileToggle');
        var menu = document.getElementById('navbarMenu');

        if (toggle && menu) {
            toggle.addEventListener('click', function () {
                menu.classList.toggle('open');
            });
        }
    }

    // =============================================
    // LIVE CLOCK
    // =============================================
    function initClock() {
        var clockEl = document.getElementById('currentTime');
        if (!clockEl) return;

        function updateTime() {
            var now = new Date();
            clockEl.textContent = now.toLocaleTimeString('en-US', {
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit'
            });
        }
        updateTime();
        setInterval(updateTime, 1000);
    }

    // =============================================
    // TASK CHECKBOX
    // =============================================
    function initTaskToggle() {
        document.addEventListener('click', function (e) {
            var target = e.target.closest('.task-check');
            if (target) {
                target.classList.toggle('done');
                if (target.classList.contains('done')) {
                    target.innerHTML = '<i class="fas fa-check"></i>';
                } else {
                    target.innerHTML = '';
                }
            }
        });
    }

    // =============================================
    // NOTIFICATIONS
    // =============================================
    function initNotifications() {
        var btn = document.getElementById('notificationBtn');
        if (btn) {
            btn.addEventListener('click', function () {
                alert('🔔 You have 3 unread notifications');
            });
        }
    }

    // =============================================
    // CHART.JS — Sales & Production
    // =============================================
    function initCharts() {
        if (typeof Chart === 'undefined') return;

        var canvas = document.getElementById('salesProductionChart');
        if (!canvas) return;

        // ✅ FIX: Purana chart destroy karein (agar exist karta hai)
        if (salesProductionChartInstance !== null) {
            salesProductionChartInstance.destroy();
            salesProductionChartInstance = null;
        }

        // ✅ FIX: Chart.js ke registry se bhi destroy karein
        var existingChart = Chart.getChart(canvas);
        if (existingChart) {
            existingChart.destroy();
        }

        var data = window.dashboardData || {};
        var salesData = data.salesChart || [];
        var productionData = data.productionChart || [];

        var labels = salesData.map(function (item) {
            return item.Label || item.label || '';
        });
        var salesValues = salesData.map(function (item) {
            return Number(item.Value || item.value || 0);
        });
        var productionValues = productionData.map(function (item) {
            return Number(item.Value || item.value || 0);
        });

        // Create gradient
        var ctx = canvas.getContext('2d');
        var salesGradient = ctx.createLinearGradient(0, 0, 0, 320);
        salesGradient.addColorStop(0, 'rgba(139, 92, 246, 0.4)');
        salesGradient.addColorStop(1, 'rgba(139, 92, 246, 0.0)');

        var prodGradient = ctx.createLinearGradient(0, 0, 0, 320);
        prodGradient.addColorStop(0, 'rgba(251, 191, 36, 0.4)');
        prodGradient.addColorStop(1, 'rgba(251, 191, 36, 0.0)');

        // ✅ FIX: Chart instance global variable mein save karein
        salesProductionChartInstance = new Chart(canvas, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Sales',
                        data: salesValues,
                        borderColor: '#8b5cf6',
                        backgroundColor: salesGradient,
                        borderWidth: 3,
                        tension: 0.4,
                        fill: true,
                        pointBackgroundColor: '#8b5cf6',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 2,
                        pointRadius: 5,
                        pointHoverRadius: 8
                    },
                    {
                        label: 'Production',
                        data: productionValues,
                        borderColor: '#fbbf24',
                        backgroundColor: prodGradient,
                        borderWidth: 3,
                        tension: 0.4,
                        fill: true,
                        pointBackgroundColor: '#fbbf24',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 2,
                        pointRadius: 5,
                        pointHoverRadius: 8
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                interaction: {
                    intersect: false,
                    mode: 'index'
                },
                plugins: {
                    legend: {
                        position: 'top',
                        align: 'end',
                        labels: {
                            usePointStyle: true,
                            padding: 20,
                            font: { family: 'Inter', size: 12, weight: '600' },
                            color: '#6b7280'
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 14,
                        cornerRadius: 12,
                        titleFont: { family: 'Inter', size: 13, weight: '600' },
                        bodyFont: { family: 'Inter', size: 12 },
                        displayColors: true,
                        borderColor: 'rgba(139, 92, 246, 0.3)',
                        borderWidth: 1
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: {
                            color: 'rgba(107, 114, 128, 0.08)',
                            drawBorder: false
                        },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) {
                                if (value >= 1000000) return (value / 1000000).toFixed(1) + 'M';
                                if (value >= 1000) return (value / 1000).toFixed(0) + 'K';
                                return value;
                            }
                        }
                    },
                    x: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af'
                        }
                    }
                }
            }
        });
    }

})();