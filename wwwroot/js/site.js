// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Slider Numara Güncelleme
var myCarousel = document.getElementById('heroCarousel')
var currentSlideElement = document.getElementById('currentSlide');
var progressBar = document.getElementById('progressBar');

if (myCarousel) {
    myCarousel.addEventListener('slide.bs.carousel', function (e) {
        // e.to, gidilecek slaytın indexidir (0'dan başlar)
        var slideIndex = e.to + 1; // 1'den başlatmak için +1 ekliyoruz

        // Numarayı güncelle (Başına 0 koyarak)
        currentSlideElement.innerText = "0" + slideIndex + ".";

        // Progress Bar'ı güncelle (4 slayt var varsayıyoruz)
        var progressPercent = (slideIndex / 4) * 100;
        progressBar.style.width = progressPercent + "%";
    })
}