// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ========================================
// NAVBAR SCROLL EFFECT - STICKY & SHADOW
// ========================================
window.addEventListener('scroll', function () {
    const navbar = document.querySelector('.main-navbar');
    const body = document.body;

    if (window.scrollY > 50) {
        navbar.classList.add('scrolled');
        body.classList.add('scrolled');
    } else {
        navbar.classList.remove('scrolled');
        body.classList.remove('scrolled');
    }
});

// ========================================
// CAROUSEL - Slider Numara Güncelleme
// ========================================
var myCarousel = document.getElementById('heroCarousel')
var currentSlideElement = document.getElementById('currentSlide');
var progressBar = document.getElementById('progressBar');

if (myCarousel) {
    myCarousel.addEventListener('slide.bs.carousel', function (e) {
        // e.to, gidilecek slaytın indexidir (0'dan başlar)
        var slideIndex = e.to + 1; // 1'den başlatmak için +1 ekliyoruz

        // Numarayı güncelle (Başına 0 koyarak)
        if (currentSlideElement) {
            currentSlideElement.innerText = "0" + slideIndex + ".";
        }

        // Progress Bar'ı güncelle (4 slayt var varsayıyoruz)
        if (progressBar) {
            var progressPercent = (slideIndex / 4) * 100;
            progressBar.style.width = progressPercent + "%";
        }
    })
}

// ========================================
// SCROLL TO TOP BUTTON
// ========================================
var scrollTopBtn = document.getElementById("scrollTopBtn");

if (scrollTopBtn) {
    window.onscroll = function () {
        // Sayfa 300px aşağı kaydırıldıysa butonu göster
        if (document.body.scrollTop > 300 || document.documentElement.scrollTop > 300) {
            scrollTopBtn.classList.add("show");
        } else {
            scrollTopBtn.classList.remove("show");
        }
    };

    // Butona tıklayınca en tepeye çık
    scrollTopBtn.addEventListener("click", function (e) {
        e.preventDefault();
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });
}

// ========================================
// SMOOTH SCROLL FOR ANCHOR LINKS
// ========================================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            e.preventDefault();
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});