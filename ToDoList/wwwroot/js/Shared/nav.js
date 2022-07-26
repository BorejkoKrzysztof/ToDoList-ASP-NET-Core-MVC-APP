
// OPEN HAMBURGER MENU
function OpenMobileMenu() {
    var mobileNavLinksBackground = document.getElementById('mobile-nav-links-background');

    mobileNavLinksBackground.classList.toggle('mobile-nav-links-bg-active');
}

document.getElementById('open-mobile-nav-links').addEventListener('click', OpenMobileMenu);
document.getElementById('open-mobile-nav-links-circle').addEventListener('click', OpenMobileMenu);

// CLOSE HAMBURGER MENU
document.getElementById('close-mobile-nav-links').addEventListener('click', () => {
    var mobileNavLinksBackground = document.getElementById('mobile-nav-links-background');

    mobileNavLinksBackground.classList.remove('mobile-nav-links-bg-active');
});

// CLOSE MENU ON RESIZING SCREEN
window.addEventListener('resize', () => {
    var mobileNavLinksBackground = document.getElementById('mobile-nav-links-background');

    if (mobileNavLinksBackground.classList.contains('mobile-nav-links-bg-active') &&
        window.innerWidth > 959) {
        mobileNavLinksBackground.classList.remove('mobile-nav-links-bg-active');
    }
});

// ONCLICK CARS NAV LINK
document.getElementById('subMenu1-nav-link').addEventListener('click', () => {

    // ROTATE
    document.getElementById('subMenu1-nav-link-arrow').classList.toggle('rotate-180');

    let hiddenMenuLauncher = document.getElementById('hidden-links-subMenu1');

    if (hiddenMenuLauncher.classList.contains('hidden')) {
        hiddenMenuLauncher.classList.remove('hidden')
    }
    else {
        hiddenMenuLauncher.classList.add('hidden');
    }
});

// SHOW CIRCLE OPEN-NAV-BUTTON AFTER SCROLL
document.addEventListener('scroll', () => {

    if (window.scrollY > window.innerHeight * 0.6) {
        document.getElementById('open-mobile-nav-links-circle').classList.remove('hidden');
    }
    else {
        document.getElementById('open-mobile-nav-links-circle').classList.add('hidden');
    }
})