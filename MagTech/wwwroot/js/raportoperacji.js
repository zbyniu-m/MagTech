var dataTable;

$(document).ready(function () {
    $('#tblData tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input type="text" placeholder="Szukaj ' + title + '" />');
    });
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({              
        dom: 'Bfrtip',
        buttons: [
            {
            extend: 'excelHtml5',
                text: '<img src="/icons/excel.png" width="28" height="28" />',
            titleAttr: 'Eksport do Excela'
            },            
        ],
        "ajax": {
            "url": "/Admin/RaportOperacji/GetAll"
        },
        "columns": [
            { "data": "data", "width": "10%" },
            { "data": "proces", "width": "15%" },
            { "data": "numerArtykulu", "width": "5%" },
            { "data": "ilosc", "width": "5%" },
            { "data": "uzytkownik", "width": "10%" },
            { "data": "stanowiskoKosztow", "width": "5%" },
            { "data": "numerMPK", "width": "5%" },
            { "data": "zadanie", "width": "5%" },
            { "data": "miejsceSkladowania", "width": "5%" },
            { "data": "wielkoscPojemnika", "width": "5%" },
            { "data": "status", "width": "5%" },
            { "data": "id", "width": "5%" }
        ],
        "order": [[0, 'desc']], 
        "language": {
            "lengthMenu": "Ilość _MENU_ rekordów na stronę",
            "zeroRecords": "Ładowanie danych",
            "info": "Strona _PAGE_ z _PAGES_",
            "infoEmpty": "Brak wyników",
            "infoFiltered": "(filtered from _MAX_ total records)",
            "search": "szukaj",
            "paginate": {
                "first": "Pierwsza",
                "previous": "Poprzedni",
                "next": "Następny",
                "last": "Ostatnia"
            },
        },
        
        // DataTable
        initComplete: function () {
            // Apply the search
            this.api().columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change clear', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        }      
    });
}

