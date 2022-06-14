//-----------
// Onload page
//-----------
window.onload = function () {
    hideLoading();
    document.querySelectorAll("[data-span]").forEach(item => {
        let table = item.parentNode.parentNode;
        let theadCount = table.querySelector("thead").rows[0].cells.length;
        item.innerHTML = `<td colspan="${theadCount}">اطلاعاتی وجود ندارد</td>`;
    });
};


//-----------
// Prevent form submitting
//-----------
preventFormSubmit();
function preventFormSubmit() {
    let forms = document.querySelectorAll("form input");
    if (forms.length) {
        for (let i = 0; i < forms.length; i++) {
            forms[i].onkeypress = function (e) {
                var key = e.charCode || e.keyCode || 0;
                if (key == 13) {
                    e.preventDefault();
                }
            }
        }
    }
}

//-----------
// Aside configuration
//-----------
asideConfigure();
function asideConfigure() {
    try {
        let result = "";
        let path = location.pathname.split("/");
        path.shift();
        for (const item of path) {
            result += `/${item.toLowerCase()}`;
        }
        // get
        let anchorTag = document.querySelector(`#aside .accordion-item .accordion-body .nav-link[href^="${result}"]`);
        let parent = anchorTag.closest(".accordion-item")
        let button = parent.querySelector(".accordion-button");
        let collapse = parent.querySelector(".accordion-collapse");

        // set
        button.classList.remove("collapsed");
        button.setAttribute("aria-expanded", "true");
        collapse.classList.add("show");
        anchorTag.classList.add("active");
    } catch (err) {
        console.log("We have error on sidebar activation");
    }
}

//-----------
// Modal
//-----------
function setModalLG() {
    document.querySelector("#createModal .modal-dialog")
        .classList.add("modal-lg");
}

function setModalXL() {
    document.querySelector("#createModal .modal-dialog")
        .classList.add("modal-xl", "modal-dialog-scrollable");
}

function viewModalAsFile(el) {
    let content = "";
    let url = el.getAttribute("data-url");
    let type = el.getAttribute("data-type");
    let title = el.getAttribute("data-title");
    url = url ? `\\${url}` : "\\panel\\images\\no-image.png";
    switch (type) {
        case null:
        case "BITMAP":
            content = `<img src="${url}" class="rounded img-fluid" alt="${title}" />`;
            break;
        case "SOUND":
            content = `<audio controls class="d-block m-auto">
                        <source src="${url}" type="audio/mpeg">
                        <source src="${url}" type="audio/ogg">
                    </audio>`;
            break;
        default:
            break;
    }
    viewBaseModal(title, content);
}

function viewModalAsText(el) {
    let title = el.getAttribute("data-title");
    let body = el.getAttribute("data-body");
    viewBaseModal(title, body);
}

function viewBaseModal(title, body) {
    let modalTag = document.getElementById('viewModal');
    var myModal = new bootstrap.Modal(modalTag, {
        keyboard: true
    });
    modalTag.querySelector(".modal-title").innerHTML = title;
    modalTag.querySelector(".modal-body").innerHTML = body;
    myModal.show();
}


//-----------
// Loading
//-----------
function hideLoading() {
    let loadingTag = document.querySelector(".ploading");
    loadingTag.classList.add("hide");
}

function showLoading() {
    let loadingTag = document.querySelector(".ploading");
    loadingTag.classList.remove("hide");
}

//-----------
// Notify
//-----------
function notifyOk(text) {
    text = text ?? "درخواست شما با موفقیت انجام گردید.";
    showNotify('success', text);
}
function notifyError(text) {
    text = text ?? "درخواست شما با خطا مواجعه گردید.";
    showNotify('error', text);
}
function notifyWarning(text) {
    showNotify('warning', text);
}
function showNotify(type, text) {
    let title = "";
    switch (type) {
        case "success":
            title = "عملیات موفق";
            break;
        case "error":
            title = "عملیات شکست";
            break;
        case "warning":
            title = "اطلاع رسانی";
            break;

        default:
            break;
    }
    new Notify({
        status: type,
        title: title,
        text: text,
        effect: 'slide',
        customIcon: '',
        customClass: '',
        showIcon: true,
        showCloseButton: false,
        autoclose: true,
        type: 1,
        speed: 500,
        autotimeout: 3000,
        gap: 20,
        distance: 20,
        position: 'right top'
    });
}

// 
// unused
// 
//-----------
// Button title
//-----------
function confAddBtnTitle(myKey) {
    let title = "افزودن";
    let params = new URLSearchParams(location.search);
    const queryObj = Object.fromEntries(params.entries());

    // if `myKey` is exists in url
    if (myKey in queryObj) {
        title = queryObj[myKey];
        sessionStorage.setItem(myKey, title);
    }
    // if `myKey` is not exist in the urk
    else {
        if ((window.sessionStorage && window.sessionStorage.getItem(myKey))) {
            title = window.sessionStorage.getItem(myKey);
        }
    }
    title = title.replace("-", " ");
    document.querySelector(".table-container .table-container-header button").innerText = title;
}

// 
// Helper
// 
function selectedOptText(el) {
    return el.options[el.selectedIndex].text;
}