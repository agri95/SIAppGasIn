

function LoadSecurity(urlApi, menuId, btnAdd, btnEdit, btnDelete) {

    $.ajax({
        method: "POST",
        url: urlApi + '?menuid=' + menuId
    })

        .done(function (result) {

            btnAdd.hide();
            btnEdit.hide();
            btnDelete.hide();

            var list = result.list;

            for (let i = 0; i < list.length; i++) {

                var code = list[i].code;
                switch (code.toLowerCase())
                {
                    case 'add':
                        btnAdd.show();
                        break;
                    case 'edit':
                        btnEdit.show();
                        break;
                    case 'delete':
                        btnDelete.show();
                        break;
                    default:
                        break;
                }
            }


        })

        .fail(function (jqXHR, textStatus) {
            alert(textStatus.Response);
        });
}


function LoadStockHeaderSecurity(urlApi, menuId, btnAdd, btnEdit, btnDelete, btnScan, btnSubmit, btnReview, btnApprove) {

    $.ajax({
        method: "POST",
        url: urlApi + '?menuid=' + menuId
    })
        .done(function (result) {

            btnAdd.hide();
            btnEdit.hide();
            btnDelete.hide();

            btnScan.hide();
            btnSubmit.hide();

            btnReview.hide();

            btnApprove.hide();

            var list = result.list;

            if (list !== undefined) {

                for (let i = 0; i < list.length; i++) {

                    var code = list[i].code;

                    switch (code.toLowerCase()) {
                        case 'add':
                            btnAdd.show();
                            break;
                        case 'edit':
                            btnEdit.show();
                            break;
                        case 'delete':
                            btnDelete.show();
                            break;
                        case 'check':
                            btnScan.show();
                            btnSubmit.show();
                            break;
                        case 'review':
                            btnReview.show();
                            break;
                        case 'approve':
                            btnApprove.show();
                            break;
                        default:
                            break;
                    }
                }
            }

        })

        .fail(function (jqXHR, textStatus) {
            alert(textStatus.Response);
        });

}


function LoadStockDetailSecurity(urlApi, menuId, btnAdd, btnEdit, btnDelete, btnSubmit, btnReCheck, btnApprove, currentStatus) {

    $.ajax({
        method: "POST",
        url: urlApi + '?menuid=' + menuId
    })

        .done(function (result) {

            btnAdd.hide();
            btnEdit.hide();
            btnDelete.hide();

            btnSubmit.hide();

            btnReCheck.hide();
            btnApprove.hide();

            var list = result.list;

            for (let i = 0; i < list.length; i++) {

                var code = list[i].code;

                switch (code.toLowerCase()) {
                    case 'check':
                        if (currentStatus.toLowerCase() === 'pending' || currentStatus.toLowerCase() === 'checked') {
                            btnAdd.show();
                            btnEdit.show();
                            btnDelete.show();
                            btnSubmit.show();
                        }
                        break;
                    case 'review':
                        if (currentStatus.toLowerCase() === 'submitted') {
                            btnApprove.show();
                            btnReCheck.show();
                        }
                        break;
                    case 'approve':
                        if (currentStatus.toLowerCase() === 'reviewed') {
                            btnApprove.show();
                            btnReCheck.show();
                        }
                        break;
                    default:
                        break;
                }
            }


        })

        .fail(function (jqXHR, textStatus) {
            alert(textStatus.Response);
        });
}


function LoadIncidentSecurity(urlApi, menuId, btnAdd, btnEdit, btnDelete, btnCheck, btnReview, btnApprove, btnAssign, btnExcalate, btnClose) {

    $.ajax({
        method: "POST",
        url: urlApi + '?menuid=' + menuId
    })

        .done(function (result) {

            btnAdd.hide();
            btnEdit.hide();
            btnDelete.hide();

            var list = result.list;

            for (let i = 0; i < list.length; i++) {

                var code = list[i].code;

                switch (code.toLowerCase()) {
                    case 'add':
                        btnAdd.show();
                        btnClose.show();
                        break;
                    case 'edit':
                        btnEdit.show();
                        break;
                    case 'delete':
                        btnDelete.show();
                        break;
                    case 'check':
                        btnCheck.show();
                        break;
                    case 'assign':
                        btnAssign.show();
                        break;
                    case 'excalate':
                        btnExcalate.show();
                        break;
                    case 'approve':
                        btnApprove.show();
                        break;
                    case 'review':
                        btnReview.show();
                        break;
                    default:
                        break;
                }
            }

        })

        .fail(function (jqXHR, textStatus) {
            alert(textStatus.Response);
        });
}


function LoadIncidentDetailSecurity(urlApi, menuId, btnResolve, btnApprove, btnReject, btnAssign, btnExcalate, currentStatus, action) {

    $.ajax({
        method: "POST",
        url: urlApi + '?menuid=' + menuId
    })

        .done(function (result) {

            action = action.toLowerCase();

            if (action === 'v') {
                $('#winAction').hide();
            }

            let btnClose = $('#btnClose');

            let btnRework = $('#btnRework');

            var list = result.list;

            for (let i = 0; i < list.length; i++) {

                var code = list[i].code;

                switch (code.toLowerCase()) {
                    case 'add':
                        if (currentStatus === 'resolved') {
                            btnClose.show();
                            btnRework.show();
                        }
                        break;
                    case 'check':
                        btnResolve.show();
                        break;
                    case 'assign':
                        btnAssign.show();
                        break;
                    case 'excalate':
                        btnExcalate.show();
                        break;
                    case 'approve':
                        btnApprove.show();
                        btnReject.show();
                        break;
                    case 'review':
                        btnResolve.show();
                        btnRework.show();
                        break;
                    default:
                        break;
                }

            }


            let engineer = $('#Engineer').val() === null  ? '' :  $.trim($('#Engineer').val());
            let technician = $('#Technician').val() === null ? '' : $.trim($('#Technician').val());                       

            if (technician !== '')
            {
                $('#winStartDate').show();
                $('#winEndDate').show();
                $('#winPhoto').show();
            }

            if (currentStatus.toLowerCase() !== 'open') {
                let ddlAsset = $('#AssetID').data('kendoDropDownList');
                ddlAsset.readonly();
            }

            switch (currentStatus.toLowerCase()) {
                case 'open':
                    btnAssign.hide();
                    btnClose.hide();
                    btnExcalate.hide();
                    btnResolve.hide();
                    btnRework.hide();
                    break;
                case 'approved':

                    $('#winPICEng').show();
                    $('#lblPIC').text('Engineer');

                    btnApprove.hide();
                    btnReject.hide();
                    btnClose.hide();
                    btnExcalate.hide();
                    btnResolve.hide();
                    btnRework.hide();

                    break;
                case 'assigned':

                    if (action === 'x') {
                        $('#winPICSec').show();
                        $('#lblPIC').text('Section Head');

                        btnAssign.hide();
                        btnApprove.hide();
                        btnClose.hide();
                        btnReject.hide();
                        btnResolve.hide();
                        btnRework.hide();
                    }
                   
                    else if (action === 's' && technician === '') {
                        $('#winPICTech').show();
                        $('#lblPIC').text('Technician');

                        btnExcalate.hide();
                        btnApprove.hide();
                        btnClose.hide();
                        btnReject.hide();
                        btnResolve.hide();
                        btnRework.hide();
                    }
                    else if (action === 'c') {
                        $('#winStartDate').show();
                        $('#winEndDate').show();
                        $('#winPhoto').show();

                        btnAssign.hide();
                        btnApprove.hide();
                        btnClose.hide();
                        btnReject.hide();
                        btnRework.hide();
                    }
                
                    break;
                case 'excalated':
                    $('#winPICTech').show();
                    $('#lblPIC').text('Technician');

                    btnExcalate.hide();
                    btnApprove.hide();
                    btnClose.hide();
                    btnReject.hide();
                    btnResolve.hide();
                    btnRework.hide();
                    break;

                case 'resolved':

                    $('#winStartDate').hide();
                    $('#winEndDate').hide();
                    $('#winPhoto').hide();

                    if (action === 'a') {
                        $('#winRating').show();
                    }
                    else
                        btnClose.hide();

                    btnAssign.hide();
                    btnExcalate.hide();
                    btnApprove.hide();
                    btnReject.hide();

                    break;

                case 'closed':
                    btnAssign.hide();
                    btnExcalate.hide();
                    btnResolve.hide();
                    btnRework.hide();

                    break;
                    
                default:
                    break;
            }

        })

        .fail(function (jqXHR, textStatus) {
            alert(textStatus.Response);
        });
}
