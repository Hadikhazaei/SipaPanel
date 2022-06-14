// 
//-----------
// select
//-----------
// 
function ajaxSelect(el) {
    showLoading();
    let selectedValue = el.value;
    let dataUrl = el.getAttribute("data-url");
    let dataParam = el.getAttribute("data-param");
    let fetchUrl = `${dataUrl}&${dataParam}=${selectedValue}`;
    let targetEl = document.getElementById(el.getAttribute("data-target"));
    targetEl.innerHTML = "";
    try {
        fetch(fetchUrl)
            .then(response => response.json())
            .then(result => {
                result.forEach(cur => {
                    targetEl.innerHTML += `<option value=${cur.value}>${cur.text}</option>`
                });
            }).catch(err => {
                console.log(err);
            });
    } catch (err) {
        console.log(err);
        targetEl.innerHTML = "";
    }
    finally{
        hideLoading();
    }
}

// 
//-----------
// checkbox
//-----------
// 
function ajaxCheckBox(el) {
    showLoading();
    fetch(el.getAttribute("data-url"))
    .then(response => response.text())
        .then(result => {
            console.log(result);
        }).catch(err => {
            console.log(err);
        });
    hideLoading();
}

// 
//-----------
// load partial view
//-----------
// 
function ajaxLoadPartialForm(el) {
    showLoading();
    let url = el.getAttribute("data-url");
    let title = el.getAttribute("data-title");
    let modalEl = document.getElementById("createModal");
    const dataCreate = modalEl.querySelector("#dataCreate");
    modalEl.querySelector(".modal-title").innerText = title;
    new bootstrap.Modal(modalEl).show();
    // 
    // 
    fetch(url).then((response) => response.text())
        .then((html) => {
            dataCreate.innerHTML = html;
            // this will be changed by pure js.
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");

            // data-picker
            if (dataCreate.querySelector("[data-picker]")) {
                datePicker(dataCreate);
            }
        }).catch((err) => {
            console.log(err);
        });
    hideLoading();
}

// 
//-----------
// data change
//-----------
//
function ajaxDataChange(el) {
        showLoading();
        fetch(el.getAttribute("data-url"))
            .then(response => {
                if (response.ok) {
                    notifyOk();
                    response.text().then((text)=>{
                        el.parentElement.querySelector("[data-content]").innerHTML=text
                    });
                    return;
                }
                response.text().then((text)=>notifyError(text));
            });
        hideLoading();
}

// 
//-----------
// datepicker
//-----------
// 
function datePicker(el) {
    let datePickers = el
        .querySelectorAll("[data-picker]");
    for (const item of datePickers) {
        // data date
        let dataDate = item.querySelector("[data-date]");
        // container
        let _container = item.querySelector("[data-container]");
        // input
        let _field = dataDate.querySelector("input");
        let _resetTrigger = dataDate.querySelector("[data-trigger] .reset-date");
        let _chooseTrigger = dataDate.querySelector("[data-trigger] .select-date");
        let dateValue = _field.value ? _field.value : null;
        let myPickDate = new Pikaday({
            firstDay: 1,
            bound: true,
            format: 'jYYYY-jMM-jDD',
            yearRange: [1930, 2050],
            field: _field,
            container: _container,
            trigger: _chooseTrigger,
            onSelect: () => {
                if (_field.hasAttribute("data-ajax")) {
                    ajaxSelect(_field);
                }
            }
        });
        myPickDate.setDate(dateValue);
        _resetTrigger.addEventListener("click", () => {
            myPickDate.setDate(null);
        });
    }
}

// document.querySelectorAll("[data-jx] [data-event]")
//     .addEventListener("click",(e)=>{
//         console.log(e.target.innerHTML);
// });
// function ajaxBegin() {
//     showLoading();
// }

// function ajaxFailure(xhr) {
//     console.log("ajaxFailure...!");
//     notifyError(xhr.responseText);
//     hideLoading();
// }

// function ajaxCompleted(xhr, url) {
//     try {
//         showLoading();
//         if (xhr.status === 200) {
//             let el = document
//                 .getElementById("dataList");
//             loadPartialAsModal(url, el);
//             notifySuccess(xhr.responseText);
//         }
//         let modalEl = document.getElementById('createModal');
//         bootstrap.Modal.getInstance(modalEl).hide();
//     } catch (err) {
//         console.log(err);
//     }
//     finally {
//         hideLoading();
//     }
// }

    // data-ajax-complete
    // data-ajax-mode
    // data-ajax-confirm	

    // data-ajax-loading-duration	
    // data-ajax-loading	

    // data-ajax-begin
    // data-ajax-failure	
    // data-ajax-success	
    // data-ajax-update

    // let rqState = parseInt(xhr.getResponseHeader("rqstate"));
    // if (status === 200 && rqState === 1) {
    // function toggleButton(state) {
    //     document.querySelector("form[data-ajax] button[type=submit]").disabled = state;
    // }