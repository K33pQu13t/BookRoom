const body = document.querySelector('body'),
    sidebar = body.querySelector('nav'),
    toggle = body.querySelector(".toggle"),
    searchBtn = body.querySelector(".search-box"),
    modeSwitch = body.querySelector(".toggle-switch"),
    modeText = body.querySelector(".mode-text"),
    collapseSection = Array.from(body.querySelectorAll("div.view-page-section > div.view-page-title"));

toggle.addEventListener("click", () => {
    sidebar.classList.toggle("close");
});

searchBtn.addEventListener("click", () => {
    sidebar.classList.remove("close");
});

modeSwitch.addEventListener("click", () => {
    body.classList.toggle("dark");

    if (body.classList.contains("dark")) {
        modeText.innerText = "Light mode";
    } else {
        modeText.innerText = "Dark mode";
    }
});

collapseSection.map((element) => { // TODO: переделать
    element.addEventListener("click", () => {
        console.log(element);
        element.parentElement.classList.toggle("collapsed");
    });
});

function toggleViewSection() { };

function updateAudiences() {
    let b = document.getElementById("filter-building");
    let buildingId = b.value;
    let f = document.getElementById("filter-floor");
    let floor = f.value;
    let t = document.getElementById("filter-type");
    let type = t.value;

    $.ajax({
        url: '/audiences/filtered',
        data: { buildingId: buildingId, floor: floor, type: type },
        success: function (response) {
            $('#audiences').html(response);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
}