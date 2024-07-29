(function () {
    let images = document.querySelectorAll('.lazyload');
    new LazyLoad(images);
    images.forEach(image => {
        image.addEventListener('load', () => {
            image.classList.remove('lazyload');
            let loader = image.parentElement.querySelector('.loader');
            if (loader)
                loader.remove();
        })
    })  
})()