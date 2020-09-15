$(document).ready(function() {

	var strIconSearch = '<i class="fas fa-search"></i>';
	var tableTitle = 'Summary of Employees';
	var tableSubTitle = 'Current Listing of Active Employees';
	var tableBS4 = $('#dtPluginExample').DataTable( {
		language: {
			lengthMenu: "Show _MENU_ Entries",
			search: strIconSearch,
			info: "Showing _START_ to _END_ of _TOTAL_ Entries"
		},
		pageLength: 10,
		searching: true,
		info: true,
		columnDefs: [
			{ targets: [0, 4], className:'text-center bg-warning' },
			{ targets: [1, 2, 3], className:'text-left bg-light' },
			{ targets: [-1, -2], className:'text-right bg-success text-white' }
        ],
		buttons: {
			buttons: [
				{ extend: 'copyHtml5', 
					text: '<i class="fas fa-copy"></i>', 
					className: 'btn-primary', 
					title: tableTitle, 
					messageTop: tableSubTitle, 
					titleAttr: 'Copy to Clipboard' 
				},
				{ extend: 'csvHtml5', 
					text: '<i class="fas fa-file-csv"></i>', 
					className: 'btn-primary', 
					titleAttr: 'Export to CSV' 
				},
				{ extend: 'excelHtml5', 
					text: '<i class="fas fa-file-excel"></i>', 
					className: 'btn-primary', 
					title: tableTitle,  
					messageTop: tableSubTitle, 
					titleAttr: 'Export to Excel' 
				},
				{ extend: 'pdfHtml5', 
					text: '<i class="fas fa-file-pdf"></i>', 
					className: 'btn-primary', 
					title: tableTitle,  
					messageTop: tableSubTitle, 
					titleAttr: 'Export to PDF' 
				},
				{ extend: 'print', 
					text: '<i class="fas fa-print"></i>', 
					className: 'btn-primary', 
					title: tableTitle,  
					messageTop: tableSubTitle, 
					titleAttr: 'Print Table' 
				},
				{ extend: 'colvis', 
					text: '<i class="fas fa-columns"></i>', 
					className: 'btn-primary', 
					titleAttr: 'Show/Hide Columns' 
				}
			],
			dom: {
			   	button: {
				className: 'btn'
				}
			}
		},
      drawCallback: function () {
        // Pagination - Add BS4-Class for Horizontal Alignment (in second of 2 columns) & Top Margin
	    $('#dtPluginExample_wrapper .col-md-7:eq(0)').addClass("d-flex justify-content-center justify-content-md-end");
	    $('#dtPluginExample_paginate').addClass("mt-3 mt-md-2");
	    $('#dtPluginExample_paginate ul.pagination').addClass("pagination-sm");

 
      }
	});

	


	// Add a row for the Title & Subtitle in front of the first row of the wrapper
	var divTitle = '' 
		+ '<div class="col-12 text-center text-md-left">'
		+ '<h4 class="text-primary">' + tableTitle + '</h4>'
		+ '<h5 class="text-primary">' + tableSubTitle + '</h5>'
		+ '<hr class="m-0 mb-4" style="border:none; background-color:rgba(0,75,141,1.0); color:rgba(0,75,141,1.0); height:1px;" />'
		+ '</div>';
	$( divTitle ).prependTo( '#dtPluginExample_wrapper .row:eq(0)' );
	
	// Insert the Button Toolbar in front of the first row of the wrapper.
	// Had to add BS4-Classes first for proper Responsive/Horizontal Alignment.
	tableBS4.buttons().container().addClass("justify-content-center justify-content-md-start mb-3");
	tableBS4.buttons().container().prependTo( '#dtPluginExample_wrapper .col-12:eq(0)' );

	// Table Header
	//    1. Remove BS4-Classes for Background set in the columnDefs Options above, 
	//    2. Add BS4-Class for White Text on Black Background 
	//    3. Reduce Font Size
	// $('#dtPluginExample thead tr th').removeClass("bg-warning bg-light bg-success").addClass("bg-dark text-white").css("font-size", "0.85rem");
	$('thead tr th').removeClass("bg-warning bg-light bg-success").addClass("bg-dark text-white").css("font-size", "0.85rem");

	// Table Footer
	//    1. Remove BS4-Classes for Background set in the columnDefs Options above, 
	//    2. Add BS4-Class for White Text on Black Background 
	//    3. Reduce Font Size
	//    4. Remove Horizontal Alignments set above and reassign for Totals.
	$('tfoot tr th').removeClass("bg-warning bg-light bg-success text-left text-center text-right").addClass("bg-dark text-white").css("font-size", "0.85rem");
	$('tfoot tr th:eq(1)').addClass("text-left");
	$('tfoot tr th:eq(6)').addClass("text-right");

	// Table Body
	//    1. Reduce Font Size
	// $('#dtPluginExample tbody tr td').css("font-size", "0.90rem");		 	// This did not work for records beyond initial rendering.  See CSS.
	// $('#dtPluginExample tbody tr td').addClass("atr-datatables-bs4-td");		// This did not work for records beyond initial rendering.  See CSS.

	$("input.form-control.form-control-sm").attr('placeholder', 'Search...');
	$("input.form-control.form-control-sm").attr('size', 30);

	// Button Toolbar & DataTables Dropdown - Add BS4-Class for Vertical Alignment (in first of 2 columns)
	$('#dtPluginExample_wrapper .col-md-6:eq(0)').addClass("align-self-end");

	// Search Box - Add BS4-Class for Vertical Alignment (in second of 2 columns)
	$('#dtPluginExample_wrapper .col-md-6:eq(1)').addClass("align-self-end");

 
});