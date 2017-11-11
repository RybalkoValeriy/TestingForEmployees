

function AddNewTitleTest() {

    if (ValidateValue($('#titleTest').val())) {

        $('#resultAddTitle').empty();
        var newTitle = $("#titleTest").val();
        bootpopup({
            title: "Ви дійсно хочете додати тему для тестування?",
            content: [newTitle],
            no: function () {
            },
            yes: function () {
                {
                    $.ajax({
                        type: 'post',
                        url: '/Admin/AddNewTitle',
                        dataType: 'json',
                        data: {
                            newTitle: newTitle
                        },
                        statusCode: {
                            200: function (data) {
                                Confirmes('Нова тема успішно додана!', newTitle);
                                $("#titleTest").val('');
                                $("#bodyTblTitle").empty();
                                RestoreTableTitle(data);
                            },
                            400: function (data) {
                                $('#titleTest').empty();
                                Confirmes("Помилка", '');
                            }
                        }
                    });
                }
            }
        });
    }
    else {
        $('#resultAddTitle').empty();
        $('#resultAddTitle').append('Заповніть тему');
        Confirmes("Заповніть тему", "")
    }
};

function Confirmes(TitleMessage, Content) {
    bootpopup({
        title: TitleMessage,
        content: [Content],
        button: ["close"]
    });
};

function DateFormatting(stringISOdate) {
    var dt = new Date(stringISOdate);
    return dateFormat = dt.getDate() + '.' + dt.getMonth() + '.' + dt.getFullYear() + ':' + dt.getHours() + '.' + dt.getMinutes() + '.' + dt.getSeconds();

};

function ValidateValue(val) {
    if (val !== NaN &&
        val !== undefined &&
        val !== '') {
        return true;
    }
}

function RestoreTableTitle(data) {
    var tablBody = $('#bodyTblTitle');
    for (var i = 0; i < data.length; i++) {
        tablBody.append(
            '<tr>' +

            '<td title=' + DateFormatting(data[i].dateAdd) + ' id="title-' + data[i].id + '">' + data[i].title + '</td>' +

            '<td>' +
            '<button class="btn btn-table" id="btnDeleteTitle" title="Видалити тему" onclick="DeleteTitle(' + data[i].id + ')">' +
            '<span class="glyphicon glyphicon-trash"></span>' +
            '</button>' +
            '</td>' +

            '<td>' +
            '<button class="btn btn-table" title="Редагувати" onclick="EditTitle(' + data[i].id + ')" id="btnEditTitle-' + data[i].id + '">' +
            '<span class="glyphicon glyphicon-edit" ></span>' +
            '</button >' +
            '<button class="btn btn-table hidden" title="Зберегти" onclick="SaveTitle(' + data[i].id + ')" id="btnSaveTitle-' + data[i].id + '">' +
            '<span class="glyphicon glyphicon-floppy-saved"></span>' +
            '</button>' +
            '</td>' +

            '<td style="width:100px;" id="CountAllQuestion-'+data[i].id+'">'+
            ''+data[i].questCount+''+
            '</td>'+

            '<td style="width:100px;">'+
            '<input type="text" value="'+data[i].countTestQuestion+'" id="CountTestQuestion-'+data[i].id+'" class="form-control" disabled style="width:100px; margin:auto; text-align:center"/>'+
            '</td>'+

            '</tr>'
        );
    }
};

function DeleteTitle(id) {

    var cont = "#title-" + id;
    var title = $(cont).text().trim();
    bootpopup({
        title: "Ви дійсно хочете видалити тему з тестування? всі питанні з даної теми будуть видалені повністю!",
        content: [title],
        no: function () {
        },
        yes: function () {
            $.ajax({
                type: 'post',
                url: '/Admin/DeleteTitle',
                dataType: 'json',
                data: {
                    id: id
                },
                statusCode: {
                    200: function (data) {
                        $('#resultAddTitle').empty();
                        $('#resultAddTitle').append('Тему успішно видалено!');
                        Confirmes("Тему успішно видалено", title);
                        $("#titleTest").empty();
                        $("#bodyTblTitle").empty();
                        RestoreTableTitle(data);
                    },
                    400: function (data) {
                        $('#resultAddTitle').empty();
                        $('#resultAddTitle').append('помилка');
                        $('#titleTest').empty();
                        Confirmes("Помилка", '');
                    }
                }
            });
        }
    });
}

function EditTitle(id) {

    $('#btnEditTitle-' + id).addClass('hidden');
    $('#btnSaveTitle-' + id).removeClass('hidden');
    var title = $('#title-' + id);
    var titleText = title.text().trim();
    title.empty();
    var inp = '<input type="text" name="EditTitle" class="form-control" value="' + titleText + '" id="inpNewTitle-' + id + '"/><span id="old-quest-' + id + '" style="display:none;">' + titleText + '</span>';
    title.append(inp);
    $('#CountTestQuestion-'+id).removeAttr('disabled');

}

function SaveTitle(id) {

    var beforeTitle = $('#old-quest-' + id).text();

    var countAllQue=$('#CountAllQuestion-'+id).text();//всего вопросов
    var countTestQue=$('#CountTestQuestion-'+id).val();//доступно для сдачи

    if (
        ValidateValue($('#inpNewTitle-' + id).val())&&
        ValidateValue(countAllQue)&&
        ValidateValue(countTestQue)&&
        !isNaN(parseInt(countAllQue,10))&&
        !isNaN(parseInt(countTestQue,10))
    ) {


        var title = $('#inpNewTitle-' + id).val();
        bootpopup({
            title: "Редагувати тему: " + beforeTitle + ' на:',
            content: [title],
            no: function () {
            },
            yes: function () {
                var newTitle = $('#inpNewTitle-' + id).val().trim();

                $.ajax({
                    type: 'post',
                    url: '/Admin/EditTitle',
                    dataType: 'json',
                    data: {
                        EditTitle: newTitle,
                        id: id,
                        CountTestQue: parseInt(countTestQue,10)
                    },
                    statusCode: {
                        200: function (data) {
                            $("#titleTest").empty();
                            $("#bodyTblTitle").empty();
                            RestoreTableTitle(data);
                            Confirmes('Тему: ' + beforeTitle + ' успішно змінено на:', title);
                        },
                        400: function (data) {
                            $('#titleTest').empty();
                            Confirmes("Помилка", '');
                        }
                    }
                });
            }

        });
    }
    else {
        Confirmes("Дані що внесені невірні, перевірте", "");
    }
}

function DeleteQuestion(idQuest) {
    if (ValidateValue(idQuest)) {
        var delQuestContext = $('#quest-text-' + idQuest).text();
        var idTitle = $('#selectTitle').val();
        bootpopup({
            title: "Відалити питання з теми?" + '<h3>' + $('#selectTitle option:selected').text() + '</h3>',
            content: [delQuestContext],
            no: function () {
            },
            yes: function () {

                $.ajax({
                    type: 'post',
                    url: '/Admin/DeleteQuest',
                    dataType: 'json',
                    data: {
                        idQuest: idQuest,
                        idTitle: idTitle,
                    },
                    statusCode: {
                        200: function (data) {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('питання знайдені!');
                            $("#bodyTblQuestion").empty();
                            Confirmes("Питання успішно видалені", "");
                            RestoreTableQuestion(data);
                            RemoveInputAll('addQuestionContainer');
                        },
                        400: function () {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('помилка');
                            Confirmes("Помилка", '');
                        }
                    }
                });
            }
        });
    }
}


function SelectTitle(id) {
    if (ValidateValue($('#selectTitle').val())) {
        CloseAnswer();
        $.ajax({
            type: 'post',
            url: '/Admin/GetGuestion',
            dataType: 'json',
            data: {
                idTitle: $('#selectTitle').val()
            },
            statusCode: {
                200: function (data) {
                    $('#resultFindQuestion').empty();
                    $("#bodyTblQuestion").empty();
                    RestoreTableQuestion(data);
                    RemoveInputAll('addQuestionContainer');
                },
                400: function (data) {
                    $('#resultFindQuestion').empty();
                    Confirmes("Помилка", '');
                }
            }
        });
    }
}

function RestoreTableQuestion(data) {
    var questionsBody = $("#bodyTblQuestion");


    for (var i = 0; i < data.length; i++) {

        var btnDel = '<button class="btn btn-table" onclick="DeleteQuestion(' + data[i].id + ')" id="quest-' + data[i].id + '"><span class="glyphicon glyphicon-trash"></span></button>';
        var btnEdit = '<button id="editquest-' + data[i].id + '" class="btn btn-table" onclick="EditQuestion(' + data[i].id + ')"><span class="glyphicon glyphicon-edit"></span></button>';
        var btnSave = '<button id="savequest-' + data[i].id + '" class="btn btn-table hidden" title="Зберегти" onclick="SaveQuestion(' + data[i].id + ')" id="btnSaveQuest-' + data[i].id + '"><span class="glyphicon glyphicon-floppy-saved"></span></button>'
        var btnSelect = '<button class="btn btn-table" id="selectBtnQuest-' + data[i].id + '" onclick="SelectQuest(' + data[i].id + ')"><span class="glyphicon glyphicon-folder-open"></span></button>';

        questionsBody.append(
            '<tr id="quest-select-' + data[i].id + '">' +
            '<td id="quest-text-' + data[i].id + '">' + data[i].questions + '</td>' +
            '<td>' + btnDel + '</td>' +
            '<td>' + btnEdit + btnSave + '</td>' +
            '<td>' + btnSelect + '</td>' +
            '<tr>'
        );
    }
};


// удаляет стиль из нутри элемента контейнера
function RemoveInnerClass(elementIdContainer, classStyleNameRemove) {
    var classStyle = $('.' + classStyleNameRemove);
    var findArrElemInClass = $('#' + elementIdContainer).find(classStyle);
    if (findArrElemInClass.length > 0) {
        $('#' + findArrElemInClass[0].id).removeClass(classStyleNameRemove);
    }

}


function SelectQuest(idQue) {

    $('#Answer-container-wrap').detach();

    RemoveInnerClass('tableQuest', 'select');

    if (ValidateValue(idQue)) {
        if ($('#quest-select-' + idQue).hasClass('select')) {
            $('#quest-select-' + idQue).removeClass('select');
        }
        else {
            $('#quest-select-' + idQue).addClass('select');
            $.ajax({
                type: 'post',
                url: '/Admin/GetQuestion',
                dataType: 'json',
                data: {
                    idQuest: idQue,
                },
                statusCode: {
                    200: function (data) {
                        $('#resultFindQuestion').empty();
                        $('#resultFindQuestion').append('відповіді змінено!');
                        AnswerRender(data, idQue);
                    },
                    400: function () {
                        $('#resultFindQuestion').empty();
                        $('#resultFindQuestion').append('помилка');
                        Confirmes("Помилка", '');
                    }
                }
            });

        }
    }
};



$(window).resize(
    function () {
        if ($('#Answer-container-wrap')) {
            $('#Answer-container-wrap').detach();

        }
    }
);

function SaveAnswer(idQue) {
    if (ValidateValue(idQue) && increm > 1) {

        var validateAnswer = true;
        var collAnswers = [];
        for (var i = 1; i < increm; i++) {
            var inpAnsw = $('#quest-inp-' + i);
            var inpState = $('#someSwitchOptionPrimary-' + i);

            if (inpAnsw && inpState) {

                if (!ValidateValue($.trim(inpAnsw.val()))) {
                    validateAnswer = false;
                    inpAnsw.attr('placeholder', 'Питання повинно бути заповнене!!!');
                }
                else {
                    var state = inpState.is(":checked") ? true : false;
                    var val = $.trim(inpAnsw.val());
                    var obj = new Object();
                    obj.Answers = val;
                    obj.State = state;
                    collAnswers.push({ Answers: val, State: state });
                }
            }
        }
        if (validateAnswer && collAnswers.length > 0) {

            var sendObject = {
                IdQue: idQue,
                CollAnswers: collAnswers
            };
            $.ajax({
                type: 'post',
                url: '/Admin/AddAnswers',
                contentType: 'application/json',
                data: JSON.stringify(sendObject),
                statusCode: {
                    200: function (data) {
                        CloseAnswer();
                        Confirmes("Відповіді успішно додані", "");
                    },
                    400: function () {
                        Confirmes("Помилка", '');
                    }
                }
            });
        }
    }
};

var increm = 1;//икремент-регулирует добавление ответов
function AnswerRender(data, idQue) {
    increm = 1;
    var containerStart = '<div id="Answer-container"  data-idQuestion="' + idQue + '">';
    var bodyAnswer = '';
    var btnAdd = '<div><button class="btn btn-table" onclick="AddOneAnswer(' + "'" + 'Answer-container' + "'" + ')"><span class="glyphicon glyphicon-plus"></span></button><button class="btn btn-table" onclick="RemoveAnswerOne(' + "'" + 'Answer-container' + "'" + ')"><span class="glyphicon glyphicon-minus"></span></button><button class="btn btn-table" onclick="SaveAnswer(' + idQue + ')"><span class="glyphicon glyphicon-floppy-saved"></span> Зберегти</button></div>';
    var containerEnd = '</div>';

    if (data.length == 0) {
        bodyAnswer = "<h4 id='answerIsNull'>По даному питанню відповідей немає</h4>";
    }

    for (var i = 0; i < data.length; i++) {

        increm = i + 1;
        var state = (data[i].state === true ? 'checked' : NaN);

        bodyAnswer = bodyAnswer + '<div class="input-group"><input type="text" value="' + data[i].title + '" id="quest-inp-' + increm + '" class="form-control" aria-describedby="tr1" ><span id="tr1" class="input-group-addon"><div class="material-switch pull-right"><input id="someSwitchOptionPrimary-' + increm + '" name="someSwitchOption001" type="checkbox" ' + state + '/><label for="someSwitchOptionPrimary-' + increm + '" class="label-primary"></label></div></span></div>';
    };

    $('#tableQuest').append('<div class="container answ-container" id="Answer-container-wrap"><div class="closeBtn"><button type="button" class="close" aria-label="Close" onclick="CloseAnswer()"><span aria-hidden="true">&times;</span></button></div>' + containerStart + bodyAnswer + containerEnd + btnAdd + '</div>');

    var select = $('#selectBtnQuest-' + idQue).offset();
    var elem = $("#Answer-container-wrap").offset({ left: select.left - 600, top: select.top });
};



function RemoveAnswerOne(containerId) {
    if ($('div').is('#' + containerId)) {
        var elem = $('#' + containerId).children(".input-group").last();
        elem.detach();

        if (increm > 1) {
            increm--;
        }
        if (increm <= 1) {
            $('#answerIsNull').empty();
            $('#answerIsNull').append('<h4 id="answerIsNull">По даному питанню відповідей немає</h4>');
        }

    }
};



function AddOneAnswer(containerId) {

    if ($('div').is('#' + containerId)) {

        var collInp = $('#' + containerId).find("input[type=text]");

        if (collInp.size() > 0) {
            increm = collInp.size()+1;
        }
        $('#answerIsNull').empty();
        $("#Answer-container-wrap").offset({ top:$("#Answer-container-wrap").offset().top-35 });
        $('#' + containerId).append(
            '<div class="input-group"><input type="text" value="" placeholder="Нова відповідь" class="form-control" aria-describedby="tr1" id="quest-inp-' + increm + '"><span id="tr1" class="input-group-addon"><div class="material-switch pull-right"><input id="someSwitchOptionPrimary-' + increm + '" name="someSwitchOption001" type="checkbox" /><label for="someSwitchOptionPrimary-' + increm + '" class="label-primary"></label></div></span></div>'
        );
        increm++;
    }
};

function CloseAnswer() {
    $('#Answer-container-wrap').detach();
};

function EditQuestion(idQue) {

    if (ValidateValue(idQue)) {
        $('#editquest-' + idQue).addClass('hidden');
        $('#savequest-' + idQue).removeClass('hidden');
        var questText = $('#quest-text-' + idQue).text().trim();
        $('#quest-text-' + idQue).empty();

        var input = '<input type="text" id="quest-text-inp' + idQue + '" value="' + questText + '" name="NewQuest" class="form-control" data-content-old="' + questText + '"/>';

        $('#quest-text-' + idQue).append(input);

    }
};

function SaveQuestion(idQue) {
    if (ValidateValue(idQue)) {
        bootpopup({
            title: "Змінити питання? - <h4>" + $('#quest-text-inp' + idQue).attr('data-content-old') + "</h4> на:",
            content: ['<h4>' + $('#quest-text-inp' + idQue).val() + '</h4>'],
            no: function () {
            },
            yes: function () {

                $.ajax({
                    type: 'post',
                    url: '/Admin/EditQuestion',
                    dataType: 'json',
                    data: {
                        idQuest: idQue,
                        EditQuest: $.trim($('#quest-text-inp' + idQue).val()),
                        idTitle: $('#selectTitle').val()
                    },
                    statusCode: {
                        200: function (data) {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('питання змінено!');
                            $("#bodyTblQuestion").empty();
                            Confirmes("Питання успішно змінені", "");
                            RestoreTableQuestion(data);
                            RemoveInputAll('addQuestionContainer');
                        },
                        400: function () {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('помилка');
                            Confirmes("Помилка", '');
                        }
                    }
                });
            }
        });
    }
};


var counter = 0;
function AddInput(elemId) {
    counter++;
    var elem = $("#" + elemId);
    var id = "'" + "wrap-quest-" + counter + "'";
    elem.append(
        '<div id="wrap-quest-' + counter + '" class="wrap-quest">' +
        '<input type="text" name="quest-' + counter + '" id="newQuest-' + counter + '" value="" class="form-control form-group-sm"/><button class="btn btn-table"' + 'onclick="RemoveOneQuest(' + id + ')">' +
        '<span class="glyphicon glyphicon-minus"></span>' +
        '</button>' +
        '</div>'
    );
};

function RemoveInputAll(elemId) {
    var elem = $("#" + elemId);
    elem.empty();
    counter = 0;
};

function RemoveOneQuest(elemId) {
    var elem = $("#" + elemId);
    elem.detach();
};

function SaveInputQuestion() {
    if (counter > 0) {
        var title = $("#selectTitle option:selected").text();
        var idTitle = $("#selectTitle").val();
        var question = [];
        for (var i = 1; i <= counter; i++) {
            var q = $('#newQuest-' + i).val();
            if (q !== NaN && q !== undefined && q !== '') {
                question.push($.trim(q));
            }
        }

        var viewContent = '<h3>' + title + '</h3><hr>';

        if (question.length > 0) {
            for (var i = 0; i < question.length; i++) {
                viewContent = viewContent + '<p>' + question[i] + '</p>';
            }
        }

        var sendObject = {
            IdTitle: idTitle,
            Questions: question
        };
        bootpopup({
            title: "Бажаете додати питання для тесту?",
            content: [viewContent],
            no: function () {
            },
            yes: function () {

                $.ajax({
                    type: 'post',
                    url: '/Admin/AddQuestionsToTitle',
                    contentType: 'application/json',
                    headers: {
                        Accept: 'application/json'
                    },
                    dataType: 'json',
                    data: JSON.stringify(sendObject),
                    statusCode: {
                        200: function (data) {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('питання знайдені!');
                            $("#bodyTblQuestion").empty();
                            Confirmes("Питання успішно додані", "");
                            RestoreTableQuestion(data);
                            RemoveInputAll('addQuestionContainer');
                        },
                        400: function () {
                            $('#resultFindQuestion').empty();
                            $('#resultFindQuestion').append('помилка');
                            Confirmes("Помилка", '');
                        }
                    }
                });
            }
        });
    }
    else {
        Confirmes('Немає питань які потрібно додати', '');
    }
};


function AccessChangeSave(userId,accessId){

    if(ValidateValue(userId)&&ValidateValue(accessId))
        {
            var accessCount=$('#select-attempts-'+accessId+' option:selected').text();
            var accessState=$('#state-access-'+accessId).is(":checked")?true:false;

            $.ajax({
                    type: 'post',
                    url: '/Access/AccessChange',
                    dataType: 'json',
                    data: {
                        AccessCount: accessCount,
                        AccessState: accessState,
                        UserId: userId,
                        AccessId: accessId
                    },
                    statusCode: {
                        200: function (data) {
                            Confirmes("Статус змінено", "");
                            window.location.reload();
                        },
                        400: function () {
                            Confirmes("Помилка", 'Кількість спроб не може бути більше 0 якщо статус вимкнений');
                        }
                    }
                });
        }
    
}