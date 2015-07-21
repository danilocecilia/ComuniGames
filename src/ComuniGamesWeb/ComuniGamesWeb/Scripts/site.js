var EstadoID = 0;
var CidadeID = 0;

$(document).ready(function () {
    listStates();

    EstadoID = $("#Users_Endereco_EstadoID").val();
    CidadeID = $("#Users_Endereco_CidadeID").val();

    $("#State").on("change", function () {
        var stateId = $(this).val();

        $("#City").empty();
        getCitiesByStateId(stateId);

    });
});

function listStates() {
    $.ajax({
        type: 'GET',
        url: 'ListAllStates',
        success: function (states) {
            $("#State").append($('<option/>').text("Selecione...").attr('value', 0)); // Adiciono um primeiro item antes de varrer lista de estados

            $.each(states.states, function (i, val) {
                $("#State").append($('<option/>').text(val.nom_estado).attr('value', val.cod_estado));
            });
            if ((EstadoID != undefined && EstadoID != '') && (CidadeID != undefined && CidadeID != '')) {
                $("#State").val(EstadoID);

                getCitiesByStateId(EstadoID)
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
}

function getCitiesByStateId(stateId) {
    $.ajax({
        type: 'POST',
        url: 'GetCitiesByStateId/' + stateId,
        success: function (cidades) {
            if (stateId == 0)
                $("#City").append($('<option/>').text("Selecione um Estado...").attr('value', 0));

            $.each(cidades.cities, function (i, val) {
                $("#City").append($('<option/>').text(val.nom_cidade).attr('value', val.cod_cidade));
            });
            if (CidadeID != undefined && CidadeID != '') {
                $("#City").val(CidadeID);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });
};
