// =============================================
// TEXCEL ERP™ — Attendance Reports Module
// Charts + Exports + AJAX Helpers
// =============================================

(function () {
    'use strict';

    // =============================================
    // HELPERS
    // =============================================
    function showToast(message, type) {
        var container = document.getElementById('toastContainer');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toastContainer';
            document.body.appendChild(container);
        }

        var toast = document.createElement('div');
        toast.className = 'toast toast-' + (type || 'info');

        var icon = 'fa-info-circle';
        if (type === 'success') icon = 'fa-check-circle';
        if (type === 'error') icon = 'fa-exclamation-circle';

        toast.innerHTML = '<i class="fas ' + icon + '"></i><span>' + message + '</span>';
        container.appendChild(toast);

        setTimeout(function () {
            toast.style.opacity = '0';
            toast.style.transition = 'opacity 0.3s';
            setTimeout(function () {
                if (toast.parentNode) toast.parentNode.removeChild(toast);
            }, 300);
        }, 3500);
    }

    function getChartData(elementId) {
        var el = document.getElementById(elementId);
        if (!el) return null;
        try {
            return JSON.parse(el.textContent);
        } catch (e) {
            console.error('Chart data parse error for ' + elementId + ':', e);
            return null;
        }
    }

    // =============================================
    // TABS — Late/OT Report
    // =============================================
    function initTabs() {
        var tabBtns = document.querySelectorAll('.tabs-header .tab-btn');
        if (!tabBtns || tabBtns.length === 0) return;

        tabBtns.forEach(function (btn) {
            btn.addEventListener('click', function () {
                var targetId = this.getAttribute('data-tab');
                if (!targetId) return;

                document.querySelectorAll('.tabs-header .tab-btn').forEach(function (b) {
                    b.classList.remove('active');
                });
                document.querySelectorAll('.tab-content').forEach(function (c) {
                    c.classList.remove('active');
                });

                this.classList.add('active');
                var targetEl = document.getElementById(targetId);
                if (targetEl) targetEl.classList.add('active');
            });
        });
    }

    // =============================================
    // CHART 1: STATUS PIE CHART (Daily Report)
    // =============================================
    function initStatusPieChart() {
        var canvas = document.getElementById('statusPieChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('statusChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var filtered = data.filter(function (d) {
            return Number(d.Value) > 0;
        });

        if (filtered.length === 0) {
            filtered = [{ Label: 'No Data', Value: 1, Color: '#e5e7eb' }];
        }

        new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels: filtered.map(function (d) { return d.Label; }),
                datasets: [{
                    data: filtered.map(function (d) { return Number(d.Value); }),
                    backgroundColor: filtered.map(function (d) { return d.Color || '#8b5cf6'; }),
                    borderWidth: 3,
                    borderColor: '#ffffff',
                    hoverBorderWidth: 5,
                    hoverOffset: 12
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '65%',
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 14,
                            font: { family: 'Inter', size: 12, weight: '600' },
                            color: '#6b7280'
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        displayColors: true,
                        callbacks: {
                            label: function (context) {
                                var total = context.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                                var pct = total > 0 ? Math.round(context.parsed / total * 100) : 0;
                                return context.label + ': ' + context.parsed + ' (' + pct + '%)';
                            }
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 2: DEPARTMENT BAR CHART (Daily Report)
    // =============================================
    function initDeptBarChart() {
        var canvas = document.getElementById('deptBarChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('deptChartData');
        if (!data || data.length === 0) {
            var ctx = canvas.getContext('2d');
            ctx.font = '14px Inter';
            ctx.fillStyle = '#9ca3af';
            ctx.textAlign = 'center';
            ctx.fillText('No data available', canvas.width / 2, canvas.height / 2);
            return;
        }

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);
        gradient.addColorStop(0, 'rgba(255, 107, 107, 0.9)');
        gradient.addColorStop(1, 'rgba(255, 107, 107, 0.4)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Present',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#ff6b6b',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'Present: ' + context.parsed.y + ' employees';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            precision: 0
                        }
                    },
                    x: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11, weight: '600' },
                            color: '#6b7280'
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 3: TOP 5 PRESENT EMPLOYEES (Horizontal Bar) — Monthly
    // =============================================
    function initTopPresentChart() {
        var canvas = document.getElementById('topPresentChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('topPresentChartData');
        if (!data || data.length === 0) {
            var ctx = canvas.getContext('2d');
            ctx.font = '14px Inter';
            ctx.fillStyle = '#9ca3af';
            ctx.textAlign = 'center';
            ctx.fillText('No data available', canvas.width / 2, canvas.height / 2);
            return;
        }

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 400, 0);
        gradient.addColorStop(0, 'rgba(161, 85, 109, 0.95)');
        gradient.addColorStop(1, 'rgba(161, 85, 109, 0.55)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Present Days',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#a1556d',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'Present: ' + context.parsed.x + ' days';
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            precision: 0
                        }
                    },
                    y: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11, weight: '600' },
                            color: '#6b7280'
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 4: TOP 5 OT EMPLOYEES (Horizontal Bar) — Monthly
    // =============================================
    function initTopOTChart() {
        var canvas = document.getElementById('topOTChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('topOTChartData');
        if (!data || data.length === 0) {
            var ctx = canvas.getContext('2d');
            ctx.font = '14px Inter';
            ctx.fillStyle = '#9ca3af';
            ctx.textAlign = 'center';
            ctx.fillText('No Overtime Data', canvas.width / 2, canvas.height / 2);
            return;
        }

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 400, 0);
        gradient.addColorStop(0, 'rgba(245, 158, 11, 0.95)');
        gradient.addColorStop(1, 'rgba(245, 158, 11, 0.55)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'OT Hours',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#f59e0b',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                indexAxis: 'y',
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'OT: ' + context.parsed.x.toFixed(1) + ' hours';
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) { return value + 'h'; }
                        }
                    },
                    y: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11, weight: '600' },
                            color: '#6b7280'
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 5: LATE TREND (Line) — Late/OT Report
    // =============================================
    function initLateTrendChart() {
        var canvas = document.getElementById('lateTrendChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('lateTrendChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 280);
        gradient.addColorStop(0, 'rgba(180, 83, 9, 0.30)');
        gradient.addColorStop(1, 'rgba(180, 83, 9, 0.0)');

        new Chart(canvas, {
            type: 'line',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Late Minutes',
                    data: data.map(function (d) { return Number(d.Value); }),
                    borderColor: '#b45309',
                    backgroundColor: gradient,
                    borderWidth: 3,
                    tension: 0.4,
                    fill: true,
                    pointBackgroundColor: '#b45309',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4,
                    pointHoverRadius: 7
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return context.parsed.y + ' minutes late';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) { return value + 'm'; }
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

    // =============================================
    // CHART 6: OT TREND (Line) — Late/OT Report
    // =============================================
    function initOTTrendChart() {
        var canvas = document.getElementById('otTrendChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('otTrendChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 280);
        gradient.addColorStop(0, 'rgba(16, 185, 129, 0.30)');
        gradient.addColorStop(1, 'rgba(16, 185, 129, 0.0)');

        new Chart(canvas, {
            type: 'line',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'OT Hours',
                    data: data.map(function (d) { return Number(d.Value); }),
                    borderColor: '#10b981',
                    backgroundColor: gradient,
                    borderWidth: 3,
                    tension: 0.4,
                    fill: true,
                    pointBackgroundColor: '#10b981',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4,
                    pointHoverRadius: 7
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return context.parsed.y.toFixed(2) + ' hours';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) { return value + 'h'; }
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

    // =============================================
    // EXPORT: CSV
    // =============================================
    function exportToCSV() {
        // Active tab ka table dhundein, warna koi bhi data-table
        var activeTab = document.querySelector('.tab-content.active');
        var table = activeTab
            ? activeTab.querySelector('.data-table')
            : document.querySelector('.data-table');

        if (!table) {
            showToast('No table found to export.', 'error');
            return;
        }

        var rows = table.querySelectorAll('tr');
        var csv = [];

        rows.forEach(function (row) {
            var cells = row.querySelectorAll('th, td');
            var rowData = [];

            cells.forEach(function (cell) {
                var text = cell.textContent.trim()
                    .replace(/"/g, '""')
                    .replace(/\s+/g, ' ');

                if (text.indexOf(',') > -1 || text.indexOf('"') > -1 || text.indexOf('\n') > -1) {
                    text = '"' + text + '"';
                }
                rowData.push(text);
            });

            csv.push(rowData.join(','));
        });

        var csvContent = '\uFEFF' + csv.join('\n');
        var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
        var url = URL.createObjectURL(blob);

        var link = document.createElement('a');
        link.href = url;
        link.download = 'Attendance_Report_' + new Date().toISOString().slice(0, 10) + '.csv';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);

        showToast('CSV exported successfully!', 'success');
    }

    // =============================================
    // EXPORT: EXCEL
    // =============================================
    function exportToExcel() {
        var activeTab = document.querySelector('.tab-content.active');
        var table = activeTab
            ? activeTab.querySelector('.data-table')
            : document.querySelector('.data-table');

        if (!table) {
            showToast('No table found to export.', 'error');
            return;
        }

        var clone = table.cloneNode(true);

        var html = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel">';
        html += '<head><meta charset="utf-8">';
        html += '<style>';
        html += 'table { border-collapse: collapse; font-family: Arial, sans-serif; }';
        html += 'th { background: #b45309; color: white; padding: 8px; border: 1px solid #ccc; font-weight: bold; text-align: left; }';
        html += 'td { padding: 6px 8px; border: 1px solid #ddd; }';
        html += 'tr:nth-child(even) { background: #f9fafb; }';
        html += '</style></head><body>';

        html += '<h2>Attendance Report</h2>';
        html += '<p>Date: ' + new Date().toLocaleDateString() + '</p>';
        html += '<table>';
        html += clone.innerHTML;
        html += '</table></body></html>';

        var blob = new Blob(['\uFEFF' + html], {
            type: 'application/vnd.ms-excel;charset=utf-8'
        });
        var url = URL.createObjectURL(blob);

        var link = document.createElement('a');
        link.href = url;
        link.download = 'Attendance_Report_' + new Date().toISOString().slice(0, 10) + '.xls';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);

        showToast('Excel exported successfully!', 'success');
    }

    // =============================================
    // EXPORT: PDF
    // =============================================
    function exportToPDF() {
        if (typeof window.jspdf === 'undefined' || !window.jspdf.jsPDF) {
            showToast('PDF library not loaded. Please refresh the page.', 'error');
            return;
        }

        var activeTab = document.querySelector('.tab-content.active');
        var table = activeTab
            ? activeTab.querySelector('.data-table')
            : document.querySelector('.data-table');

        if (!table) {
            showToast('No table found to export.', 'error');
            return;
        }

        var jsPDF = window.jspdf.jsPDF;
        var doc = new jsPDF('l', 'mm', 'a4');

        // Header
        doc.setFillColor(180, 83, 9);
        doc.rect(0, 0, 297, 20, 'F');

        doc.setTextColor(255, 255, 255);
        doc.setFontSize(16);
        doc.setFont('helvetica', 'bold');
        doc.text('TEXCEL ERP — Attendance Report', 14, 13);

        doc.setTextColor(80, 80, 80);
        doc.setFontSize(10);
        doc.setFont('helvetica', 'normal');
        doc.text('Generated: ' + new Date().toLocaleString(), 14, 26);

        // Table data
        var head = [];
        var headRow = [];
        table.querySelectorAll('thead th').forEach(function (th) {
            headRow.push(th.textContent.trim());
        });
        head.push(headRow);

        var body = [];
        table.querySelectorAll('tbody tr').forEach(function (tr) {
            var row = [];
            tr.querySelectorAll('td').forEach(function (td) {
                row.push(td.textContent.trim().replace(/\s+/g, ' '));
            });
            body.push(row);
        });

        doc.autoTable({
            head: head,
            body: body,
            startY: 32,
            theme: 'striped',
            headStyles: {
                fillColor: [180, 83, 9],
                textColor: 255,
                fontStyle: 'bold',
                fontSize: 9
            },
            bodyStyles: {
                fontSize: 8,
                textColor: [55, 65, 81]
            },
            alternateRowStyles: {
                fillColor: [249, 250, 251]
            },
            margin: { top: 32, left: 8, right: 8 },
            styles: {
                cellPadding: 2,
                lineColor: [229, 231, 235],
                lineWidth: 0.1
            },
            didDrawPage: function (data) {
                doc.setFontSize(8);
                doc.setTextColor(150);
                var pageStr = 'Page ' + doc.internal.getNumberOfPages();
                doc.text(pageStr, 285, 205, { align: 'right' });
            }
        });

        doc.save('Attendance_Report_' + new Date().toISOString().slice(0, 10) + '.pdf');
        showToast('PDF exported successfully!', 'success');
    }

    // =============================================
    // AJAX HELPER
    // =============================================
    function loadReportAjax(url, params, callback) {
        var query = [];
        for (var key in params) {
            if (params.hasOwnProperty(key) && params[key]) {
                query.push(encodeURIComponent(key) + '=' + encodeURIComponent(params[key]));
            }
        }
        var fullUrl = url + (query.length ? '?' + query.join('&') : '');

        fetch(fullUrl, {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Accept': 'application/json'
            }
        })
            .then(function (r) {
                if (!r.ok) throw new Error('HTTP ' + r.status);
                return r.json();
            })
            .then(function (data) {
                if (typeof callback === 'function') callback(null, data);
            })
            .catch(function (err) {
                console.error('AJAX error:', err);
                if (typeof callback === 'function') callback(err, null);
            });
    }


    // =============================================
    // CHART 7: DEPT ATTENDANCE % (Department Report)
    // =============================================
    function initDeptAttendanceChart() {
        var canvas = document.getElementById('deptAttendanceChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('deptAttendanceChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);
        gradient.addColorStop(0, 'rgba(5, 150, 105, 0.95)');
        gradient.addColorStop(1, 'rgba(5, 150, 105, 0.55)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Attendance %',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#059669',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'Attendance: ' + context.parsed.y.toFixed(1) + '%';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) { return value + '%'; }
                        }
                    },
                    x: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11, weight: '600' },
                            color: '#6b7280'
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 8: DEPT OT HOURS (Department Report)
    // =============================================
    function initDeptOTChart() {
        var canvas = document.getElementById('deptOTChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('deptOTChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);
        gradient.addColorStop(0, 'rgba(245, 158, 11, 0.95)');
        gradient.addColorStop(1, 'rgba(245, 158, 11, 0.55)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'OT Hours',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#f59e0b',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'OT: ' + context.parsed.y.toFixed(1) + ' hrs';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: {
                            font: { family: 'Inter', size: 11 },
                            color: '#9ca3af',
                            callback: function (value) { return value + 'h'; }
                        }
                    },
                    x: {
                        grid: { display: false },
                        ticks: {
                            font: { family: 'Inter', size: 11, weight: '600' },
                            color: '#6b7280'
                        }
                    }
                }
            }
        });
    }


    // =============================================
    // CHART 9: EMPLOYEE PRESENT (Graph Analytics)
    // =============================================
    function initEmployeePresentChart() {
        var canvas = document.getElementById('employeePresentChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('employeePresentChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);
        gradient.addColorStop(0, 'rgba(124, 58, 237, 0.95)');
        gradient.addColorStop(1, 'rgba(124, 58, 237, 0.55)');

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Present Days',
                    data: data.map(function (d) { return Number(d.Value); }),
                    backgroundColor: gradient,
                    borderColor: '#7c3aed',
                    borderWidth: 2,
                    borderRadius: 8,
                    borderSkipped: false
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'Present: ' + context.parsed.y + ' days';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: { font: { family: 'Inter', size: 11 }, color: '#9ca3af', precision: 0 }
                    },
                    x: {
                        grid: { display: false },
                        ticks: { font: { family: 'Inter', size: 10, weight: '600' }, color: '#6b7280', maxRotation: 45 }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 10: MONTHLY TREND (Graph Analytics)
    // =============================================
    function initMonthlyTrendChart() {
        var canvas = document.getElementById('monthlyTrendChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('monthlyTrendChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var ctx = canvas.getContext('2d');
        var gradient = ctx.createLinearGradient(0, 0, 0, 300);
        gradient.addColorStop(0, 'rgba(168, 85, 247, 0.35)');
        gradient.addColorStop(1, 'rgba(168, 85, 247, 0.0)');

        new Chart(canvas, {
            type: 'line',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: [{
                    label: 'Present Records',
                    data: data.map(function (d) { return Number(d.Value); }),
                    borderColor: '#a855f7',
                    backgroundColor: gradient,
                    borderWidth: 3,
                    tension: 0.4,
                    fill: true,
                    pointBackgroundColor: '#a855f7',
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 5,
                    pointHoverRadius: 8
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { display: false },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                return 'Present: ' + context.parsed.y + ' records';
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: { font: { family: 'Inter', size: 11 }, color: '#9ca3af', precision: 0 }
                    },
                    x: {
                        grid: { display: false },
                        ticks: { font: { family: 'Inter', size: 11 }, color: '#9ca3af' }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 11: STATUS DISTRIBUTION (Graph Analytics)
    // =============================================
    function initStatusDistributionChart() {
        var canvas = document.getElementById('statusDistributionChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('statusDistributionChartData');
        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        var filtered = data.filter(function (d) {
            return Number(d.Value) > 0;
        });

        if (filtered.length === 0) return;

        new Chart(canvas, {
            type: 'doughnut',
            data: {
                labels: filtered.map(function (d) { return d.Label; }),
                datasets: [{
                    data: filtered.map(function (d) { return Number(d.Value); }),
                    backgroundColor: filtered.map(function (d) { return d.Color || '#7c3aed'; }),
                    borderWidth: 3,
                    borderColor: '#ffffff',
                    hoverBorderWidth: 5,
                    hoverOffset: 12
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                cutout: '60%',
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 14,
                            font: { family: 'Inter', size: 12, weight: '600' },
                            color: '#6b7280'
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 },
                        callbacks: {
                            label: function (context) {
                                var total = context.dataset.data.reduce(function (a, b) { return a + b; }, 0);
                                var pct = total > 0 ? Math.round(context.parsed / total * 100) : 0;
                                return context.label + ': ' + context.parsed + ' (' + pct + '%)';
                            }
                        }
                    }
                }
            }
        });
    }

    // =============================================
    // CHART 12: DEPARTMENT COMPARISON (Stacked Bar)
    // =============================================
    function initDeptComparisonChart() {
        var canvas = document.getElementById('deptComparisonChart');
        if (!canvas || typeof Chart === 'undefined') return;

        var data = getChartData('deptComparisonChartData');
        var labels = getChartData('deptComparisonLabels');
        var series = getChartData('deptComparisonSeries');

        if (!data || data.length === 0) return;

        var existingChart = Chart.getChart(canvas);
        if (existingChart) existingChart.destroy();

        // Build stacked datasets
        var colors = [
            { bg: 'rgba(16, 185, 129, 0.85)', border: '#10b981' },     // Present
            { bg: 'rgba(239, 68, 68, 0.85)', border: '#ef4444' },      // Absent
            { bg: 'rgba(59, 130, 246, 0.85)', border: '#3b82f6' }      // Leave
        ];

        var datasets = [];
        if (series && series.length > 0) {
            for (var i = 0; i < series.length; i++) {
                var values = data.map(function (d) {
                    return (d.Values && d.Values[i]) ? Number(d.Values[i]) : 0;
                });

                datasets.push({
                    label: series[i],
                    data: values,
                    backgroundColor: colors[i] ? colors[i].bg : 'rgba(124, 58, 237, 0.85)',
                    borderColor: colors[i] ? colors[i].border : '#7c3aed',
                    borderWidth: 2,
                    borderRadius: 6,
                    borderSkipped: false
                });
            }
        }

        new Chart(canvas, {
            type: 'bar',
            data: {
                labels: data.map(function (d) { return d.Label; }),
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            usePointStyle: true,
                            padding: 14,
                            font: { family: 'Inter', size: 12, weight: '600' },
                            color: '#6b7280'
                        }
                    },
                    tooltip: {
                        backgroundColor: 'rgba(26, 26, 46, 0.95)',
                        padding: 12,
                        cornerRadius: 10,
                        titleFont: { family: 'Inter', size: 13, weight: '700' },
                        bodyFont: { family: 'Inter', size: 12 }
                    }
                },
                scales: {
                    x: {
                        stacked: true,
                        grid: { display: false },
                        ticks: { font: { family: 'Inter', size: 11, weight: '600' }, color: '#6b7280' }
                    },
                    y: {
                        stacked: true,
                        beginAtZero: true,
                        grid: { color: 'rgba(107, 114, 128, 0.08)', drawBorder: false },
                        ticks: { font: { family: 'Inter', size: 11 }, color: '#9ca3af', precision: 0 }
                    }
                }
            }
        });
    }

    // =============================================
    // INIT
    // =============================================
    function init() {
        // Charts — Daily
        initStatusPieChart();
        initDeptBarChart();

        // Charts — Monthly
        initTopPresentChart();
        initTopOTChart();

        // Charts — Late/OT (NEW)
        initLateTrendChart();
        initOTTrendChart();

        // Charts — Department (NEW)
        initDeptAttendanceChart();
        initDeptOTChart();

        // Charts — Graph Analytics (NEW)
        initEmployeePresentChart();
        initMonthlyTrendChart();
        initStatusDistributionChart();
        initDeptComparisonChart();


        // Tabs — Late/OT (NEW)
        initTabs();

        // Export buttons
        var btnPDF = document.getElementById('btnExportPDF');
        var btnExcel = document.getElementById('btnExportExcel');
        var btnCSV = document.getElementById('btnExportCSV');

        if (btnPDF) btnPDF.addEventListener('click', exportToPDF);
        if (btnExcel) btnExcel.addEventListener('click', exportToExcel);
        if (btnCSV) btnCSV.addEventListener('click', exportToCSV);

        console.log('Attendance Reports JS loaded');
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

    // Expose to window
    window.AttendanceReports = {
        exportToCSV: exportToCSV,
        exportToExcel: exportToExcel,
        exportToPDF: exportToPDF,
        showToast: showToast,
        loadReportAjax: loadReportAjax
    };

})();