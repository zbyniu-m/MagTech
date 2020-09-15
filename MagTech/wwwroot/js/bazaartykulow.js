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
        //"scrollX": true,       
        "ajax": {
            "url": "/Admin/BazaArtykulow/GetAll"
        },        
        "columns": [                
            { "data": "numerArtykulu", "width": "10%" },
            { "data": "nazwaArtykulu", "width": "15%"},
            { "data": "stanWMiejscuSkladowania", "width": "5%"},
            { "data": "stanMinimalny", "width": "5%" },
            { "data": "dodatkoweInformacje", "width": "10%" },
            { "data": "grupaMaterialowa", "width": "5%"},
            { "data": "numerRegalu", "width": "5%" },
            { "data": "numerPolki", "width": "5%"},
            { "data": "numerPrzedzialu", "width": "5%"},
            { "data": "numerSzeregu", "width": "5%" },
            { "data": "wielkoscPojemnika", "width": "5%" },
            { "data": "oznaczenieFifo", "width": "5%" },
            { "data": "typ", "width": "5%" },
            { "data": "tag", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/BazaArtykulow/Edit/${data}" class="btn btn-info" style="cursor:pointer" data-toggle="tooltip" data-placement="bottom" title="" data-original-title="Edytuj">
                                    <img src="/icons/edit-button.png" width="16" height="16" />
                                </a>                                
                            </div>
                           `;
                }, "width": "10%"
            }          
                     
        ],
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

