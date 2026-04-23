/*
Template Name: Lexa - Admin & Dashboard Template
Author: Themesbrand
Website: https://themesbrand.com/
Contact: themesbrand@gmail.com
File: Dashboard
*/

// const { data } = require("jquery");

function getChartColorsArray(chartId) {
    if (document.getElementById(chartId) !== null) {
        var colors = document.getElementById(chartId).getAttribute("data-colors");
        if (colors) {
            colors = JSON.parse(colors);
            return colors.map(function (value) {
                var newValue = value.replace(" ", "");
                if (newValue.indexOf(",") === -1) {
                    var color = getComputedStyle(document.documentElement).getPropertyValue(
                        newValue
                    );
                    if (color) return color;
                    else return newValue;
                } else {
                    var val = value.split(",");
                    if (val.length == 2) {
                        var rgbaColor = getComputedStyle(
                            document.documentElement
                        ).getPropertyValue(val[0]);
                        rgbaColor = "rgba(" + rgbaColor + "," + val[1] + ")";
                        return rgbaColor;
                    } else {
                        return newValue;
                    }
                }
            });
        } else {
            console.warn('data-colors Attribute not found on:', chartId);
        }
    }
}

// Morris Code
function ChartColorChange(chartupdate, chartId) {
    document.querySelectorAll(".theme-color").forEach(function (item) {
        item.addEventListener("click", function (event) {
            setTimeout(function() {
                var updatechartColors = getChartColorsArray(chartId);
                if(chartupdate.options){
                    if(chartupdate.options["colors"]){
                        chartupdate.options["colors"] = updatechartColors;
                    }else if(chartupdate.options["lineColors"]){
                        chartupdate.options["lineColors"] = updatechartColors;
                    }else if(chartupdate.options["barColors"]){
                        chartupdate.options["barColors"] = updatechartColors;
                    }
                    chartupdate.redraw();
                }
            }, 0);
        });
    });
}

// Sparkline Code
function ChartColorChangeSparkLine(series, chartupdate, chartId) {
    document.querySelectorAll(".theme-color").forEach(function (item) {
        item.addEventListener("click", function (event) {
            setTimeout(function() {
                var updatechartColors = getChartColorsArray(chartId);
                chartupdate.barColor = updatechartColors;
                $('#'+ chartId).sparkline(series, chartupdate);
            }, 0);
        });
    });
}

!function($) {
    "use strict";

    var Dashboard = function() {};
    
    //creates area chart
    Dashboard.prototype.createAreaChart = function (element, pointSize, lineWidth, data, xkey, ykeys, labels, lineColors) {
        var areaChart = Morris.Area({
            element: element,
            pointSize: 0,
            lineWidth: 1,
            data: data,
            xkey: xkey,
            ykeys: ykeys,
            labels: labels,
            resize: true,
            gridLineColor: 'rgba(108, 120, 151, 0.1)',
            hideHover: 'auto',
            lineColors: lineColors,
            fillOpacity: .9,
            behaveLikeLine: true
        });
        ChartColorChange(areaChart,'morris-area-example');
    },

    //creates Donut chart
    Dashboard.prototype.createDonutChart = function (element, data, colors, select) {
        // var total = data.reduce((sum, item) => sum + item.value, 0);
        var donutChart = Morris.Donut({
            element: element,
            data: data,
            resize: true,
            colors: colors,
            formatter: function (value) {
                // return Math.round((value / total) * 100) + '%';
                return value + '%';
            },
        //    click: function (i, row) {
        //        console.log('Clicked donut segment:', row);

        //        const donutModal = new bootstrap.Modal(document.getElementById('donutModal'));
        //        donutModal.show();
        //        //$('#donutModal').modal('show')
        //    }
        });
        donutChart.select(select);
        ChartColorChange(donutChart,'morris-donut');

        //return donutChart;
    },

    //creates Stacked chart
    Dashboard.prototype.createStackedChart  = function(element, data, xkey, ykeys, labels, lineColors) {
        var barChart = Morris.Bar({
            element: element,
            data: data,
            xkey: xkey,
            ykeys: ykeys,
            stacked: true,
            labels: labels,
            hideHover: 'auto',
            resize: true, //defaulted to true
             gridLineColor: 'rgba(108, 120, 151, 0.1)',
            barColors: lineColors
        });
        ChartColorChange(barChart,'morris-bar-stacked');
    },
    
    Dashboard.prototype.init = function() {
        
        //creating area chart
        var areaEXChartColors = getChartColorsArray("morris-area-example");
        if (areaEXChartColors) {
        var $areaData = [
            {y: '2011', a: 0, b: 0, c:0},
            {y: '2012', a: 150, b: 45, c:15},
            {y: '2013', a: 60, b: 150, c:195},
            {y: '2014', a: 180, b: 36, c:21},
            {y: '2015', a: 90, b: 60, c:360},
            {y: '2016', a: 75, b: 240, c:120},
            {y: '2017', a: 30, b: 30, c:30}
        ];
        this.createAreaChart('morris-area-example', 0, 0, $areaData, 'y', ['a', 'b', 'c'], ['Series A', 'Series B', 'Series C'], areaEXChartColors);
    }
        //creating donut chart
    //     var donutEXChartColors = getChartColorsArray("morris-donut");
    //     if (donutEXChartColors) {
    //     var $donutData = [
    //         {label: "Barang Tersedia", value: 156},
    //         {label: "Tidak Tersedia", value: 44},
    //     ];
    //     this.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
    //     //window.myDonut = this.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
    // }

        var barStackedChartColors = getChartColorsArray("morris-bar-stacked");
        if (barStackedChartColors) {
        var $stckedData  = [
            { y: '2005', a: 45, b: 180},
            { y: '2006', a: 75,  b: 65},
            { y: '2007', a: 100, b: 90},
            { y: '2008', a: 75,  b: 65},
            { y: '2009', a: 100, b: 90},
            { y: '2010', a: 75,  b: 65},
            { y: '2011', a: 50,  b: 40},
            { y: '2012', a: 75,  b: 65},
            { y: '2013', a: 50,  b: 40},
            { y: '2014', a: 75,  b: 65},
            { y: '2015', a: 100, b: 90},
            { y: '2016', a: 80, b: 65}
        ];
        this.createStackedChart('morris-bar-stacked', $stckedData, 'y', ['a', 'b'], ['Series A', 'Series B'], barStackedChartColors);
    }
    },
    //init
    $.Dashboard = new Dashboard, $.Dashboard.Constructor = Dashboard
    loadData();
}(window.jQuery),

//initializing 
function($) {
    "use strict";
    $.Dashboard.init();
}(window.jQuery);


var sparklineChart1Colors = getChartColorsArray("sparkline");
if (sparklineChart1Colors) {
    var series = [8, 6, 4, 7, 10, 12, 7, 4, 9, 12, 13, 11, 12];
    var chartoption = {
        type: 'bar',
        height: '130',
        barWidth: '10',
        barSpacing: '7',
        barColor: '#7A6FBE'
    };
    var demo = $('#sparkline').sparkline(series, chartoption);
    ChartColorChangeSparkLine(series,chartoption,'sparkline');
}

// Tambahan dari SIGAP

// document.addEventListener("DOMContentLoaded", function () {
//     document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(function (trigger) {
//       const icon = trigger.querySelector("i");
//       const targetId = trigger.getAttribute("href") || trigger.dataset.bsTarget;
//       const collapseEl = document.querySelector(targetId);

//       if (!collapseEl || !icon) return;

//       collapseEl.addEventListener("show.bs.collapse", function () {
//         icon.classList.remove("mdi-chevron-right");
//         icon.classList.add("mdi-chevron-down");
//       });

//       collapseEl.addEventListener("hide.bs.collapse", function () {
//         icon.classList.remove("mdi-chevron-down");
//         icon.classList.add("mdi-chevron-right");
//       });
//     });
// });

$(function () {
    $('.activity-feed .feed-item-list > div').each(function () {
        let itemRows = $(this).find('.d-flex.justify-content-between');
        if (itemRows.length > 3) {
            itemRows.slice(3).addClass('d-none');
            //$(this).append('<div class="text-muted text-center" style="font-weight: bold;">..........</div>');
            $(this).append('<a href="javascript:void(0);" class="toggle-switch d-flex justify-content-center text-muted text-center" style="font-weight: bold;">Lihat Detail</a>');
        }
    });

    $('.toggle-switch').on('click', function () {
        var $container = $(this).closest('div');
        var $hiddenItems = $container.find('.d-flex.justify-content-between.d-none');

        if ($hiddenItems.length > 0) {
            $hiddenItems.removeClass('d-none');
            $(this).text('Tutup');
        } else {
            $container.find('.d-flex.justify-content-between').slice(3).addClass('d-none'); // Hide after 3
            $(this).text('Lihat Detail');
        }
    });

});

function loadData() {
    const tbody = $("#donutModal tbody");

    // Optionally show spinner over donut chart as well
    // const donutContainer = $("#morris-donut");
    // donutContainer.html(`
    //     <div class="d-flex justify-content-center align-items-center" style="height:200px;">
    //         <div class="spinner-border text-secondary" role="status"></div>
    //     </div>
    // `);

    const donutContainer = $("#donut-container");
    // donutContainer.html(`
    //     <div class="d-flex flex-column justify-content-center align-items-center" style="height: 250px;">
    //         <div class="spinner-border text-secondary" role="status" style="width: 5rem; height: 5rem;"></div>
    //         <span class="mt-3 fw-semibold fs-6 text-secondary">Memuat data...</span>
    //     </div>
    // `);

    // $("#wrapper-dashboard").append(`
    //     <div class="spinner-overlay">
    //         <div class="spinner-border" role="status"></div>
    //     </div>
    // `);

    // let wrapper = $("#wrapper-dashboard");
    // let originalContent = wrapper.html();
    // wrapper.html(`
    //     <div class="d-flex justify-content-center p-5">
    //         <div class="spinner-border" role="status"></div>
    //     </div>
    // `);
    // wrapper.removeAttr("hidden");

    // $.ajax({
    //     url: '/api/Dashboard/readiness', // change to your actual endpoint
    //     method: 'GET',
    //     dataType: 'json',
    //     success: function (res) {
    //         tbody.empty();
    //         let totalJumlahBarang = 0;
    //         let totalMSLBarang = 0;

    //         const categories = res.data || [];

    //         if (categories.length === 0) {
    //             tbody.html(`<tr><td colspan="3" class="text-center text-muted py-3">Tidak ada data.</td></tr>`);
    //             donutContainer.html(`
    //                 <p class="text-black">Tidak ada data.</p>
    //             `);
    //             return;
    //         }

    //         categories.forEach((category, index) => {
    //             if (!category.itemDto || category.itemDto.length === 0) return;

    //             const collapseId = `category${index + 1}`;
    //             let totalJumlahBarangPerKategori = 0;
    //             let totalMSLBarangPerKategori = 0;

    //             const itemsRows = (category.itemDto || []).map((item, i) => {
    //                 const readiness = Math.round(
    //                     (item.jumlah_barang / (item.msl_barang || 1)) * 100
    //                 );
    //                 totalJumlahBarangPerKategori += item.jumlah_barang;
    //                 totalMSLBarangPerKategori += item.msl_barang;

    //                 return `
    //                     <tr>
    //                         <td>${i + 1}</td>
    //                         <td>${item.nama_barang}</td>
    //                         <td>${item.jumlah_barang}</td>
    //                         <td>${item.msl_barang}</td>
    //                         <td><span class="badge ${getBadgeClass(readiness)}">${readiness}%</span></td>
    //                     </tr>
    //                 `;
    //             }).join("");

    //             const readinessCategory = Math.round(
    //                 (totalJumlahBarangPerKategori / (totalMSLBarangPerKategori || 1)) * 100
    //             );

    //             const categoryRow = `
    //                 <tr data-bs-toggle="collapse" data-bs-target="#${collapseId}">
    //                     <td>
    //                         <a href="#${collapseId}" data-bs-toggle="collapse" aria-expanded="false" aria-controls="${collapseId}">
    //                             <i class="mdi mdi-custom mdi-chevron-right text-black"></i>
    //                         </a>
    //                     </td>
    //                     <td class="text-start">${category.namakategoribar}</td>
    //                     <td><span class="badge ${getBadgeClass(readinessCategory)}">${readinessCategory}%</span></td>
    //                 </tr>
    //             `;

    //             const collapseRow = `
    //                 <tr class="collapse" id="${collapseId}">
    //                     <td colspan="3" class="p-0">
    //                         <div class="collapse ps-custom" id="${collapseId}">
    //                             <table class="table table-bordered table-sm mb-0 align-middle nested-table">
    //                                 <thead class="table-dark text-white">
    //                                     <tr>
    //                                         <th>No</th>
    //                                         <th>Nama Material</th>
    //                                         <th>Stock</th>
    //                                         <th>MSL</th>
    //                                         <th>Readiness Stock</th>
    //                                     </tr>
    //                                 </thead>
    //                                 <tbody class="tbody-custom">${itemsRows}</tbody>
    //                             </table>
    //                         </div>
    //                     </td>
    //                 </tr>
    //             `;

    //             totalJumlahBarang += totalJumlahBarangPerKategori;
    //             totalMSLBarang += totalMSLBarangPerKategori;

    //             tbody.append(categoryRow + collapseRow);
    //         });

    //         bindCollapseIcons();
    //         // var donutEXChartColors = getChartColorsArray("morris-donut");
    //         // if (donutEXChartColors) {
    //         //     var $donutData = [
    //         //         {label: "Barang Tersedia", value: 144},
    //         //         {label: "Tidak Tersedia", value: 56},
    //         //     ];
    //         //     $.Dashboard.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
    //         //     //window.myDonut = this.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
    //         // }

    //         // donutContainer.html(`
    //         //     <div id="morris-donut"
    //         //         data-colors='["#3BFF3B","#FF3B3E"]'
    //         //         class="morris-charts morris-donut"
    //         //         dir="ltr">
	// 		// 	</div>
	// 		// 	<p class="text-black">Klik untuk melihat detail</p>
    //         // `);

    //         // $("#wrapper-dashboard .spinner-overlay").remove();

    //         wrapper.html(originalContent);

    //         var divider = totalMSLBarang || 1;
    //         if(totalJumlahBarang <= 0)
    //             $.Dashboard.createDonutChart('morris-donut', [{label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)}], ['#FF3B3E'], 0);
    //         else if(totalJumlahBarang >= totalMSLBarang)
    //             $.Dashboard.createDonutChart('morris-donut', [{label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)}], ['#3BFF3B'], 0);
    //         else
    //             $.Dashboard.createDonutChart('morris-donut', [
    //                 {label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)},
    //                 {label: "Tidak Tersedia", value: Math.round(((totalMSLBarang-totalJumlahBarang)/divider) * 100)}
    //             ], ['#3BFF3B','#FF3B3E'], 0);
    //     },
    //     error: function (xhr) {
    //         let res = xhr.responseJSON;
    //         let msg = res?.message || 'Gagal memuat data readiness.';

    //         if (res?.errors) {
    //             // Get first error key and its first message
    //             const firstKey = Object.keys(res.errors)[0];
    //             const firstError = res.errors[firstKey]?.[0];

    //             if (firstError) {
    //                 msg = firstError; // show only that message
    //             }
    //         }

    //         donutContainer.html(`
	// 			<p class="text-danger">Gagal memuat data readiness.</p>
    //         `);
    //     }
    // });

    let wrapper = $("#wrapper-dashboard");
    // let originalContent = wrapper.html();
    wrapper.before(`
        <div class="d-flex justify-content-center p-5">
            <div class="spinner-border" role="status"></div>
        </div>
    `);

    const requestDashboardData = $.ajax({
        url: '/api/Dashboard',
        method: 'GET',
        dataType: 'json',
    }).fail(function(xhr) {
        let res = xhr.responseJSON;
        let msg = res?.message || 'Gagal memuat data dashboard.';

        if (res?.errors) {
            // Get first error key and its first message
            const firstKey = Object.keys(res.errors)[0];
            const firstError = res.errors[firstKey]?.[0];

            if (firstError) {
                msg = firstError; // show only that message
            }
        }

        $('#error-dashboard').removeAttr("hidden");
    });

    const requestReadinessData = $.ajax({
        url: '/api/Dashboard/readiness',
        method: 'GET',
        dataType: 'json',
    }).fail(function(xhr) {
        let res = xhr.responseJSON;
        let msg = res?.message || 'Gagal memuat data readiness.';

        if (res?.errors) {
            // Get first error key and its first message
            const firstKey = Object.keys(res.errors)[0];
            const firstError = res.errors[firstKey]?.[0];

            if (firstError) {
                msg = firstError; // show only that message
            }
        }

        donutContainer.html(`
            <p class="text-danger">Gagal memuat data readiness.</p>
        `);
    });

    $.when(requestDashboardData, requestReadinessData).done(function (resDashboardData, resReadinessData) {
        const dashboardData = resDashboardData[0].data || {};
        if (dashboardData.length === 0) {
            $('#transact_in_cnt').html('Tidak ada data.');
            $('#transact_out_cnt').html('Tidak ada data.');
            $('#transact_pending_cnt').html('Tidak ada data.');
            $('#item_low_stock_cnt').html('Tidak ada data.');
            $('#pemasukkan-terbaru').append(`<li>Tidak ada data pemasukkan terbaru.</li>`);
            $('#pengeluaran-terbaru').append(`<li>Tidak ada data pengeluaran terbaru.</li>`);
            $('#tombol-riwayat-transaksi').hide();
            $('#tombol-riwayat-stock').hide();
            return;
        }

        $('#transact_in_cnt').html(dashboardData.transact_in_cnt);
        $('#transact_out_cnt').html(dashboardData.transact_out_cnt);
        $('#transact_pending_cnt').html(dashboardData.transact_pending_cnt);
        $('#item_low_stock_cnt').html(dashboardData.item_low_stock_cnt);
        renderLatestActivities(dashboardData);

        // ========================================================================================

        tbody.empty();
        let totalJumlahBarang = 0;
        let totalMSLBarang = 0;

        const categories = resReadinessData[0].data || [];

        if (categories.length === 0) {
            tbody.html(`<tr><td colspan="3" class="text-center text-muted py-3">Tidak ada data.</td></tr>`);
            donutContainer.html(`
                <p class="text-black">Tidak ada data.</p>
            `);
            return;
        }

        categories.forEach((category, index) => {
            if (!category.itemDto || category.itemDto.length === 0) return;

            const collapseId = `category${index + 1}`;
            let totalJumlahBarangPerKategori = 0;
            let totalMSLBarangPerKategori = 0;

            const itemsRows = (category.itemDto || []).map((item, i) => {
                const readiness = Math.round(
                    (item.jumlah_barang / (item.msl_barang || 1)) * 100
                );
                totalJumlahBarangPerKategori += item.jumlah_barang;
                totalMSLBarangPerKategori += item.msl_barang;

                return `
                    <tr>
                        <td>${i + 1}</td>
                        <td>${item.nama_barang}</td>
                        <td>${item.jumlah_barang - item.booked_qty}</td>
                        <td>${item.booked_qty}</td>
                        <td>${item.jumlah_barang}</td>
                        <td>${item.msl_barang}</td>
                        <td><span class="badge ${getBadgeClass(readiness)}">${readiness}%</span></td>
                    </tr>
                `;
            }).join("");

            const readinessCategory = Math.round(
                (totalJumlahBarangPerKategori / (totalMSLBarangPerKategori || 1)) * 100
            );

            const categoryRow = `
                <tr data-bs-toggle="collapse" data-bs-target="#${collapseId}">
                    <td>
                        <a href="#${collapseId}" data-bs-toggle="collapse" aria-expanded="false" aria-controls="${collapseId}">
                            <i class="mdi mdi-custom mdi-chevron-right text-black"></i>
                        </a>
                    </td>
                    <td class="text-start">${category.namakategoribar}</td>
                    <td><span class="badge ${getBadgeClass(readinessCategory)}">${readinessCategory}%</span></td>
                </tr>
            `;

            const collapseRow = `
                <tr class="collapse" id="${collapseId}">
                    <td colspan="3" class="p-0">
                        <div class="collapse ps-custom" id="${collapseId}">
                            <table class="table table-bordered table-sm mb-0 align-middle nested-table">
                                <thead class="table-dark text-white">
                                    <tr>
                                        <th style="width:5%">No</th>
                                        <th style="width:30%">Nama Material</th>
                                        <th style="width:10%">Stok</th>
                                        <th style="width:10%">Booked</th>
                                        <th style="width:10%">Jumlah</th>
                                        <th style="width:10%">MSL</th>
                                        <th style="width:25%">Readiness Stock</th>
                                    </tr>
                                </thead>
                                <tbody class="tbody-custom">${itemsRows}</tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            `;

            totalJumlahBarang += totalJumlahBarangPerKategori;
            totalMSLBarang += totalMSLBarangPerKategori;

            tbody.append(categoryRow + collapseRow);
        });

        bindCollapseIcons();
        // var donutEXChartColors = getChartColorsArray("morris-donut");
        // if (donutEXChartColors) {
        //     var $donutData = [
        //         {label: "Barang Tersedia", value: 144},
        //         {label: "Tidak Tersedia", value: 56},
        //     ];
        //     $.Dashboard.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
        //     //window.myDonut = this.createDonutChart('morris-donut', $donutData, donutEXChartColors, 0);
        // }

        donutContainer.html(`
            <div id="morris-donut"
                data-colors='["#3BFF3B","#FF3B3E"]'
                class="morris-charts morris-donut"
                dir="ltr">
            </div>
            <p class="text-black">Klik untuk melihat detail</p>
        `);

        // $("#wrapper-dashboard .spinner-overlay").remove();
        wrapper.prev().remove();
        wrapper.removeAttr("hidden");

        var divider = totalMSLBarang || 1;
        if(totalJumlahBarang <= 0)
            $.Dashboard.createDonutChart('morris-donut', [{label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)}], ['#FF3B3E'], 0);
        else if(totalJumlahBarang >= totalMSLBarang)
            $.Dashboard.createDonutChart('morris-donut', [{label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)}], ['#3BFF3B'], 0);
        else
            $.Dashboard.createDonutChart('morris-donut', [
                {label: "Barang Tersedia", value: Math.round((totalJumlahBarang/divider) * 100)},
                {label: "Tidak Tersedia", value: Math.round(((totalMSLBarang-totalJumlahBarang)/divider) * 100)}
            ], ['#3BFF3B','#FF3B3E'], 0);

        // wrapper.html(originalContent);
    });
}

function getBadgeClass(readiness) {
    if (readiness >= 115) return "bg-success";
    if (readiness >= 100) return "bg-warning";
    return "bg-danger";
}

function bindCollapseIcons() {
    document.querySelectorAll('[data-bs-toggle="collapse"]').forEach(function (trigger) {
        const icon = trigger.querySelector("i");
        const targetId = trigger.getAttribute("href") || trigger.dataset.bsTarget;
        const collapseEl = document.querySelector(targetId);

        if (!collapseEl || !icon) return;

        collapseEl.addEventListener("show.bs.collapse", function () {
            icon.classList.remove("mdi-chevron-right");
            icon.classList.add("mdi-chevron-down");
        });

        collapseEl.addEventListener("hide.bs.collapse", function () {
            icon.classList.remove("mdi-chevron-down");
            icon.classList.add("mdi-chevron-right");
        });
    });
}

// function renderLatestActivities(data) {

//     // a. Pemasukkan
//     const pemasukkanContainer = $("#pemasukkan-terbaru");
//     pemasukkanContainer.empty();

//     if (!data.latestTransactionInDto || data.latestTransactionInDto.length === 0) {
//         pemasukkanContainer.append(`<li>Tidak ada data Pemasukkan.</li>`);
//     } else {
//         data.latestTransactionInDto.forEach(t => {
//             pemasukkanContainer.append(generateFeedItemHTML(t));
//         });
//     }

//     // b. Pengeluaran
//     const pengeluaranContainer = $("#pengeluaran-terbaru");
//     pengeluaranContainer.empty();

//     if (!data.latestTransactionOutDto || data.latestTransactionOutDto.length === 0) {
//         pengeluaranContainer.append(`<li>Tidak ada data Pengeluaran.</li>`);
//     } else {
//         data.latestTransactionOutDto.forEach(t => {
//             pengeluaranContainer.append(generateFeedItemHTML(t));
//         });
//     }
// }

function renderLatestActivities(data) {

    // Ambil 5 data teratas (karena backend sudah sorted)
    const pemasukkan = (data.latestTransactionInDto || []).slice(0, 5);
    const pengeluaran = (data.latestTransactionOutDto || []).slice(0, 5);

    const pemasukkanContainer = $("#pemasukkan-terbaru");
    const pengeluaranContainer = $("#pengeluaran-terbaru");

    pemasukkanContainer.empty();
    pengeluaranContainer.empty();

    if (pemasukkan.length === 0) {
        pemasukkanContainer.append(`<li>Tidak ada data Pemasukkan.</li>`);
    } else {
        pemasukkan.forEach(t => pemasukkanContainer.append(generateFeedItemHTML(t)));
    }

    if (pengeluaran.length === 0) {
        pengeluaranContainer.append(`<li>Tidak ada data Pengeluaran.</li>`);
    } else {
        pengeluaran.forEach(t => pengeluaranContainer.append(generateFeedItemHTML(t)));
    }
}

function generateFeedItemHTML(trans) {

    const date = formatDateToWIB(trans.created_at);

    // Build detail items (barang)
    let detailsHTML = "";
    let status = trans.status;
    trans.transactionDetailDto.forEach(d => {
        const barang = d.itemDto?.nama_barang ?? "-";
        const jumlah = d.jumlah_bar ?? 0;
        const uom = d.itemDto?.satuanbar_id ?? "";

        detailsHTML += `
            <div class="d-flex justify-content-between gap-3">
                <p>${barang}</p>
                <p>${jumlah}${uom.toLowerCase()}</p>
            </div>
        `;
    });

    switch (status) {
        case 'Approval Section Head Pending':
            status = `<span class="badge bg-warning">Pending</span>`;
            break;
        case 'Approval Section Head Safety Pending':
            status = `<span class="badge bg-warning">Pending</span>`;
            break;
        case 'Menunggu Konfirmasi Gudang':
            status = `<span class="badge bg-warning">Pending</span>`;
            break;
        case 'Approval Section Head Rejected':
            status = `<span class="badge bg-danger">Rejected</span>`;
            break;
        case 'Approval Section Head Safety Rejected':
            status = `<span class="badge bg-danger">Rejected</span>`;
            break;
        case 'Approval Gudang Rejected':
            status = `<span class="badge bg-danger">Rejected</span>`;
            break;
        case 'Done':
            status = `<span class="badge bg-success">Done</span>`;
            break;
    }

    return `
        <li class="feed-item">
            <div class="feed-item-list">
                <div>
                    <div class="date">${date}</div>
                    <div class="d-flex mb-3">${status}</div>
                    ${detailsHTML}
                </div>
            </div>
        </li>
    `;
}

function formatDateToWIB(input) {
    if (!input && input !== 0) return '';

    // If already a Date
    if (input instanceof Date) {
        if (isNaN(input)) return 'Invalid Date';
        return formatFromDate(input);
    }

    const str = String(input).trim();

    // Detect ISO-ish string first
    if (/^\d{4}-\d{2}-\d{2}T/.test(str) || /^\d{4}\/\d{2}\/\d{2}/.test(str)) {
        const d = new Date(str);
        if (!isNaN(d)) return formatFromDate(d);
    }

    // Try parse "dd/mm/yyyy hh:mm:ss" (or without time)
    const [datePart, timePart = '00:00:00'] = str.split(' ');
    const datePieces = datePart.split('/');

    if (datePieces.length === 3) {
        const day = parseInt(datePieces[0], 10);
        const month = parseInt(datePieces[1], 10) - 1; // JS months 0-11
        const year = parseInt(datePieces[2], 10);

        const timePieces = timePart.split(':').map(x => parseInt(x, 10) || 0);
        const hour = timePieces[0] || 0;
        const minute = timePieces[1] || 0;
        const second = timePieces[2] || 0;

        const d = new Date(year, month, day, hour, minute, second);
        if (!isNaN(d)) return formatFromDate(d);
    }

    // Fallback: try letting Date parse it
    const fallback = new Date(str);
    if (!isNaN(fallback)) return formatFromDate(fallback);

    return 'Invalid Date';

    function pad(n) { return String(n).padStart(2, '0'); }

    function formatFromDate(d) {
        // If you need to convert to Asia/Jakarta specifically (and user's browser is in another TZ),
        // you'd need to use Intl with timeZone or a lib like luxon. For typical cases where server timestamps
        // are already local to the desired TZ, this is fine:
        const hh = pad(d.getHours());
        const mm = pad(d.getMinutes());
        const dd = pad(d.getDate());
        const mo = pad(d.getMonth() + 1);
        const yy = d.getFullYear();
        return `${hh}.${mm} WIB, ${dd}/${mo}/${yy}`;
    }
}

// function formatDateToWIB(str) {
//     const dt = new Date(str);
//     const options = {
//         hour: "2-digit",
//         minute: "2-digit",
//         hour12: false,
//         timeZone: "Asia/Jakarta"
//     };

//     const time = dt.toLocaleTimeString("id-ID", options);
//     const date = dt.toLocaleDateString("id-ID");

//     return `${time} WIB, ${date}`;
// }

// function loadDonutModalData() {
//     const tbody = $("#donutModal tbody");
//     // tbody.html(`
//     //     <tr><td colspan="3" class="text-center py-4">
//     //         <div class="spinner-border text-info" role="status"></div>
//     //         <span class="ms-2">Memuat data...</span>
//     //     </td></tr>
//     // `);
//     Swal.fire({
//         title: 'Memuat data...',
//         didOpen: () => Swal.showLoading(),
//         allowOutsideClick: false,
//         showConfirmButton: false
//     });

//     $.ajax({
//         url: '/api/Dashboard/readiness', // change to your actual endpoint
//         method: 'GET',
//         dataType: 'json',
//         success: function (res) {
//             tbody.empty();
//             const categories = res.data || [];

//             if (categories.length === 0) {
//                 tbody.html(`<tr><td colspan="3" class="text-center text-muted py-3">Tidak ada data.</td></tr>`);
//                 return;
//             }

//             categories.forEach((category, index) => {
//                 const collapseId = `category${index + 1}`;
//                 let totalJumlahBarangPerKategori = 0;
//                 let totalMSLBarangPerKategori = 0;

//                 const itemsRows = (category.itemDto || []).map((item, i) => {
//                     const readiness = Math.round(
//                         (item.jumlah_barang / (item.msl_barang || 1)) * 100
//                     );
//                     totalJumlahBarangPerKategori += item.jumlah_barang;
//                     totalMSLBarangPerKategori += item.msl_barang;

//                     return `
//                         <tr>
//                             <td>${i + 1}</td>
//                             <td>${item.nama_barang}</td>
//                             <td>${item.jumlah_barang}</td>
//                             <td>${item.msl_barang}</td>
//                             <td><span class="badge ${getBadgeClass(readiness)}">${readiness}%</span></td>
//                         </tr>
//                     `;
//                 }).join("");

//                 const readinessCategory = Math.round(
//                     (totalJumlahBarangPerKategori / (totalMSLBarangPerKategori || 1)) * 100
//                 );

//                 const categoryRow = `
//                     <tr data-bs-toggle="collapse" data-bs-target="#${collapseId}">
//                         <td>
//                             <a href="#${collapseId}" data-bs-toggle="collapse" aria-expanded="false" aria-controls="${collapseId}">
//                                 <i class="mdi mdi-custom mdi-chevron-right text-black"></i>
//                             </a>
//                         </td>
//                         <td class="text-start">${category.namakategoribar}</td>
//                         <td><span class="badge ${getBadgeClass(readinessCategory)}">${readinessCategory}%</span></td>
//                     </tr>
//                 `;

//                 const collapseRow = `
//                     <tr class="collapse" id="${collapseId}">
//                         <td colspan="3" class="p-0">
//                             <div class="collapse ps-custom" id="${collapseId}">
//                                 <table class="table table-bordered table-sm mb-0 align-middle nested-table">
//                                     <thead class="table-dark text-white">
//                                         <tr>
//                                             <th>No</th>
//                                             <th>Nama Material</th>
//                                             <th>Stock</th>
//                                             <th>MSL</th>
//                                             <th>Readiness Stock</th>
//                                         </tr>
//                                     </thead>
//                                     <tbody>${itemsRows}</tbody>
//                                 </table>
//                             </div>
//                         </td>
//                     </tr>
//                 `;

//                 tbody.append(categoryRow + collapseRow);
//             });

//             bindCollapseIcons();

//         },
//         error: function (xhr) {
//             // tbody.html(`
//             //     <tr><td colspan="3" class="text-center text-danger py-3">
//             //         Gagal memuat data: ${xhr.responseJSON?.message || xhr.statusText}
//             //     </td></tr>
//             // `);                
//             Swal.close();
//             let res = xhr.responseJSON;
//             let msg = res?.message || 'Gagal memuat data readiness.';

//             if (res?.errors) {
//                 // Get first error key and its first message
//                 const firstKey = Object.keys(res.errors)[0];
//                 const firstError = res.errors[firstKey]?.[0];

//                 if (firstError) {
//                     msg = firstError; // show only that message
//                 }
//             }

//             Swal.fire('Error', msg, 'error');
//         }
//     });
// }
