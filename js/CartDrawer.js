const cartDrawer = document.getElementById("cart-drawer");
const cartWrapper = cartDrawer.querySelector(".cart__wrapper");
const cartContent = cartDrawer.querySelector(".cart__content");
const cartClose = cartDrawer.querySelector(".cart__close");
const cartOpenBtns = document.querySelectorAll(".btn-cart");
cartContent.classList.add("translate-x-full");

cartWrapper.addEventListener("click", (e) => {
    if (e.target === cartWrapper) {
        if (window.innerWidth < 768)
            document.documentElement.classList.remove("prevent-scroll");
        cartContent.classList.add("translate-x-full");
        cartWrapper.style.setProperty("--tw-bg-opacity", 0);
        setTimeout(() => {
            cartWrapper.classList.add("hidden");
        }, 300);
    }
});

cartClose.addEventListener("click", () => {
    if (window.innerWidth < 768)
        document.documentElement.classList.remove("prevent-scroll");
    cartContent.classList.add("translate-x-full");
    cartWrapper.style.setProperty("--tw-bg-opacity", 0);
    setTimeout(() => {
        cartWrapper.classList.add("hidden");
    }, 300);
});

document.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        if (window.innerWidth < 768)
            document.documentElement.classList.remove("prevent-scroll");
        cartContent.classList.add("translate-x-full");
        cartWrapper.style.setProperty("--tw-bg-opacity", 0);
        setTimeout(() => {
            cartWrapper.classList.add("hidden");
        }, 300);
    }
});

cartOpenBtns.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        const id = e.currentTarget.getAttribute("data-id");
        if (id == null) {
            loadCart();
            if (window.innerWidth < 768)
                document.documentElement.classList.add("prevent-scroll");
            cartWrapper.classList.remove("hidden");
            setTimeout(() => {
                cartContent.classList.remove("translate-x-full");
                cartWrapper.style.setProperty("--tw-bg-opacity", 0.5);
            }, 300);
            return;
        }
        $.ajax({
            url: "/AddToCart",
            type: "POST",
            data: { id: id },
            success: function (response) {
                loadCart(true);
                if (window.innerWidth < 768)
                    document.documentElement.classList.add("prevent-scroll");
                cartWrapper.classList.remove("hidden");
                setTimeout(() => {
                    cartContent.classList.remove("translate-x-full");
                    cartWrapper.style.setProperty("--tw-bg-opacity", 0.5);
                }, 300);
            }
        });
    });
});

function renderCart(currentCart, addToCart = false) {
    const badges = document.querySelectorAll(".badge");

    if (currentCart.items.length == 0) {
        cartWrapper.classList.add("cart-empty");
        badges.forEach((badge) => {
            const badgeInner = badge.querySelector(".badge-inner");
            if (badgeInner) badgeInner.remove();
        });
    } else {
        cartWrapper.classList.remove("cart-empty");
        badges.forEach((badge) => {
            const badgeInner = badge.querySelector(".badge-inner");
            if (badgeInner) {
                badgeInner.textContent = currentCart.items.reduce(
                    (acc, item) => acc + item.Quantity,
                    0,
                );
            } else {
                const badgeInner = document.createElement("span");
                badgeInner.classList.add("badge-inner");
                badgeInner.textContent = currentCart.items.reduce(
                    (acc, item) => acc + item.Quantity,
                    0,
                );
                badge.appendChild(badgeInner);
            }
        });
    }

    const addSuccessMsg =
        '<div class="notification success"><svg class="w-6 h-6" fill="none" stroke="currentColor" focusable="false" aria-hidden="true" viewBox="0 0 24 24"><path d="M9 16.2 4.8 12l-1.4 1.4L9 19 21 7l-1.4-1.4z"></path></svg><div class="ml-3">Product added to cart successfully</div></div>';

    const cartBody = cartContent.querySelector(".cart__body");
    cartBody.innerHTML =
        currentCart.items.length > 0
            ? `
  <div class="cart__items">
    ${addToCart ? addSuccessMsg : ""}
    ${currentCart.items
                .map(
                    (item, index) =>
                        `
        <div class="cart-item" data-index=${index} data-product-id=${item.Id}>
          <div class="cart-item__inner flex items-start">
            <div class="cart-item__image">
              <a href=${"/Product/" + item.Handle} class="block" style="--aspect-ratio:1.499">
                <img src=${item.MainImg} alt=${item.Name} />
              </a>
            </div>
            <div class="cart-item__info">
              <a href=${"/Product/" + item.Handle} class="font-bold hover:underline">${item.Name}</a>
              
              <div class="cart-item__prices">
                <div class="cart-item__price text-color-regular-price">
                  <span class="money" data-product-price=${item.Price}>$${item.Price / 100} USD</span>
                </div>
              </div>
              <div class="flex items-center mt-[10px] justify-between">
                <div class="cart-item__quantity flex justify-between rounded">
                  <button class="cart-item__btn" data-id=${item.Id} data-qty-change="dec">-</button>
                  <input class="py-1 cart-item__qty_input w-9 text-center" type="number" name="quantity" id="quantity${index}" data-id=${item.Id} value=${item.Quantity} min="0">
                  <button class="cart-item__btn" data-id=${item.Id} data-qty-change="inc">+</button>
                </div>
                <button class="cart-item__remove underline p-2 ml-2" data-id=${item.Id}>Remove</button>
              </div>
            </div>
          </div>
        </div>
        `,
                )
                .join("")}
    </div>
    `
            : "<div class='cart-empty-msg'><p>Your cart is currently empty.</p></div>";

    const cartSubtotal = cartContent.querySelector(".cart__subtotal-price");
    cartSubtotal.innerHTML = `<span class="money" data-product-price=${currentCart.subtotal}></span>`;
    document.querySelectorAll(".currency-selector").forEach((selector) => {
        selector.dispatchEvent(new Event("change"));
    });

    const cartItemDecBtns = cartContent.querySelectorAll(
        ".cart-item__btn[data-qty-change='dec']",
    );
    const cartItemIncBtns = cartContent.querySelectorAll(
        ".cart-item__btn[data-qty-change='inc']",
    );
    const cartItemQtyInputs = cartContent.querySelectorAll(
        ".cart-item__qty_input",
    );
    const cartItemRemoveBtns = cartContent.querySelectorAll(".cart-item__remove");

    cartItemDecBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            if (currentCart.items[itemIndex].Quantity > 1) {
                $.ajax({
                    url: "/UpdateCart",
                    type: "POST",
                    data: { id: id, quantity: currentCart.items[itemIndex].Quantity - 1 },
                    success: function (data) {
                        loadCart();
                    },
                });
            } else {
                $.ajax({
                    url: "/RemoveFromCart",
                    type: "POST",
                    data: { id: id },
                    success: function (data) {
                        loadCart();
                    },
                });
            }
        });
    });

    cartItemIncBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            $.ajax({
                url: "/UpdateCart",
                type: "POST",
                data: { id: id, quantity: currentCart.items[itemIndex].Quantity + 1 },
                success: function (data) {
                    loadCart();
                },
            });
        });
    });

    cartItemQtyInputs.forEach((input) => {
        input.addEventListener("change", (e) => {
            const id = e.target.getAttribute("data-id");
            const newQty = parseInt(e.target.value);
            if (newQty > 0) {
                $.ajax({
                    url: "/UpdateCart",
                    type: "POST",
                    data: { id: id, quantity: newQty },
                    success: function (data) {
                        loadCart();
                    },
                });
            } else {
                $.ajax({
                    url: "/RemoveFromCart",
                    type: "POST",
                    data: { id: id },
                    success: function (data) {
                        loadCart();
                    },
                });
            }
        });
    });

    cartItemRemoveBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            $.ajax({
                url: "/RemoveFromCart",
                type: "POST",
                data: { id: id },
                success: function (data) {
                    loadCart();
                },
            });
        });
    });

    if (addToCart && currentCart.items.length > 0) {
        setTimeout(() => {
            cartBody.querySelector(".notification")?.classList.add("show");
        }, 500);
        setTimeout(() => {
            cartBody.querySelector(".notification")?.classList.remove("show");
        }, 3000);
        setTimeout(() => {
            cartBody.querySelector(".notification")?.remove();
        }, 3300);
    }
}

function loadCart(addToCart = false) {
    $.ajax({
        url: "/CartList",
        type: "GET",
        success: function (data) {
            renderCart(data, addToCart)
        },
    });
}

loadCart();

document.querySelectorAll(".btn-atc").forEach((btn) => {
    btn.addEventListener("click", (e) => {
        //const productId = product.id;
        //const productTitle = product.title;
        //let currentVariant = null;
        //if (variantId) {
        //    currentVariant =
        //        product.variants.find(
        //            (variant) => parseInt(variant.id) === parseInt(variantId),
        //        ) || product.variants[0];
        //} else {
        //    currentVariant = product.variants[0];
        //}
        //const currentvariantId = currentVariant.id;
        //const variantPrice = currentVariant.price;
        //const variantOptions = currentVariant.options
        //    .filter((option) => option !== "Default Title")
        //    .map((option, index) => {
        //        return {
        //            name: product.options[index].name,
        //            value: option,
        //        };
        //    });
        //const productImg = currentVariant.featured_image
        //    ? currentVariant.featured_image.src
        //    : product.featured_image;
        //const productAlt = currentVariant.featured_image
        //    ? currentVariant.featured_image.alt
        //    : product.title;
        //const productUrl = product.url;

        //var currentCart = JSON.parse(sessionStorage.getItem("cart")) || {
        //    items: [],
        //    subtotal: 0,
        //};

        //const itemIndex = currentCart.items.findIndex(
        //    (item) => item.id === productId && item.variantId === currentvariantId,
        //);

        //if (itemIndex === -1) {
        //    currentCart.items.push({
        //        id: productId,
        //        title: productTitle,
        //        price: variantPrice,
        //        img: productImg,
        //        alt: productAlt,
        //        url: productUrl,
        //        variantId: currentvariantId,
        //        options_with_values: variantOptions ? variantOptions : null,
        //        qty: 1,
        //    });
        //} else {
        //    currentCart.items[itemIndex].Quantity += 1;
        //}

        //currentCart.subtotal = currentCart.items.reduce(
        //    (acc, item) => acc + item.Price * item.Quantity,
        //    0,
        //);

        //sessionStorage.setItem("cart", JSON.stringify(currentCart));

        loadCart(true);

        if (window.innerWidth < 768)
            document.documentElement.classList.add("prevent-scroll");
        cartWrapper.classList.remove("hidden");
        setTimeout(() => {
            cartContent.classList.remove("translate-x-full");
            cartWrapper.style.setProperty("--tw-bg-opacity", 0.5);
        }, 300);
    });
});

document.querySelectorAll(".buy-one-click .paypal").forEach((btn) => {
    btn.addEventListener("click", (e) => {
        //const productId = product.id;
        //const productTitle = product.title;
        //let currentVariant = null;
        //if (variantId) {
        //    currentVariant =
        //        product.variants.find(
        //            (variant) => parseInt(variant.id) === parseInt(variantId),
        //        ) || product.variants[0];
        //} else {
        //    currentVariant = product.variants[0];
        //}
        //const currentvariantId = currentVariant.id;
        //const variantPrice = currentVariant.price;
        //const variantOptions = currentVariant.options
        //    .filter((option) => option !== "Default Title")
        //    .map((option, index) => {
        //        return {
        //            name: product.options[index].name,
        //            value: option,
        //        };
        //    });
        //const productImg = currentVariant.featured_image
        //    ? currentVariant.featured_image.src
        //    : product.featured_image;
        //const productAlt = currentVariant.featured_image
        //    ? currentVariant.featured_image.alt
        //    : product.title;
        //const productUrl = product.url;

        //const checkout = {
        //    id: productId,
        //    title: productTitle,
        //    price: variantPrice,
        //    img: productImg,
        //    alt: productAlt,
        //    url: productUrl,
        //    variantId: currentvariantId,
        //    options_with_values: variantOptions ? variantOptions : null,
        //    qty: 1,
        //}

        //sessionStorage.setItem("checkout", JSON.stringify(checkout));
        window.location.href = "/checkout/?payment=paypal";
    });
});

document.querySelectorAll(".buy-one-click .more-options").forEach((btn) => {
    btn.addEventListener("click", (e) => {
        //const productId = product.id;
        //const productTitle = product.title;
        //let currentVariant = null;
        //if (variantId) {
        //    currentVariant =
        //        product.variants.find(
        //            (variant) => parseInt(variant.id) === parseInt(variantId),
        //        ) || product.variants[0];
        //} else {
        //    currentVariant = product.variants[0];
        //}
        //const currentvariantId = currentVariant.id;
        //const variantPrice = currentVariant.price;
        //const variantOptions = currentVariant.options
        //    .filter((option) => option !== "Default Title")
        //    .map((option, index) => {
        //        return {
        //            name: product.options[index].name,
        //            value: option,
        //        };
        //    });
        //const productImg = currentVariant.featured_image
        //    ? currentVariant.featured_image.src
        //    : product.featured_image;
        //const productAlt = currentVariant.featured_image
        //    ? currentVariant.featured_image.alt
        //    : product.title;
        //const productUrl = product.url;

        //const checkout = {
        //    id: productId,
        //    title: productTitle,
        //    price: variantPrice,
        //    img: productImg,
        //    alt: productAlt,
        //    url: productUrl,
        //    variantId: currentvariantId,
        //    options_with_values: variantOptions ? variantOptions : null,
        //    qty: 1,
        //}

        //sessionStorage.setItem("checkout", JSON.stringify(checkout));
        window.location.href = "/checkout";
    });
});

document.querySelector(".cart__checkout").addEventListener("click", () => {
    if (sessionStorage.getItem("checkout"))
        sessionStorage.removeItem("checkout");
    window.location.href = "/checkout";
});