window.sidebarMenu = {
    init: function () {
        const toggle = document.getElementById("menuToggle");
        const sidebar = document.getElementById("sidebar");
        const overlay = document.getElementById("sidebarOverlay");

        if (!toggle || !sidebar || !overlay) {
            return;
        }

        toggle.onclick = function () {
            sidebar.classList.toggle("open");
            overlay.classList.toggle("open");
        };

        overlay.onclick = function () {
            sidebar.classList.remove("open");
            overlay.classList.remove("open");
        };

        const links = sidebar.querySelectorAll(".nav-link");

        links.forEach(function (link) {
            link.onclick = function () {
                sidebar.classList.remove("open");
                overlay.classList.remove("open");
            };
        });
    }
};