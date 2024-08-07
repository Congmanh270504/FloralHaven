
function saveCart() {
    const cart = JSON.parse(sessionStorage.getItem("cart"));
    if (cart) {
        $.ajax({
            url: "/SaveCartList",
            type: "POST",
            data: { items: cart.items, len: cart.items.length },
            success: function (data) {
                loadCart();
            },
        });
    }
}



const cartWrapper = document.querySelector(".cart__wrapper");
const cartTable = cartWrapper.querySelector(".cart__table");

function loadCart(addToCart = false) {
    (function () {
        $.ajax({
            url: "/cartlist",
            type: "GET",
            success: function (data) {
                sessionStorage.setItem("cart", JSON.stringify(data));

            },
        });
    })();

    let currentCart = JSON.parse(sessionStorage.getItem("cart")) || {
        items: [],
        subtotal: 0,
    };

    const badges = document.querySelectorAll(".badge");

    if (currentCart.items.length == 0) {
        document.body.classList.add("cart-empty");
        badges.forEach((badge) => {
            const badgeInner = badge.querySelector(".badge-inner");
            if (badgeInner) badgeInner.remove();
        });
    } else {
        document.body.classList.remove("cart-empty");
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

    const cartBody = cartTable.querySelector(".cart__table-body");
    cartBody.innerHTML =
        currentCart.items.length > 0
            ? `<div class="cart__items">
          ${addToCart ? addSuccessMsg : ""}
          ${currentCart.items
                .map(
                    (item, index) =>
                        `
            <div class="cart__item cart-item flex" data-index=${index} data-product-id=${item.id} >
              <div class="cart__table-col cart__table-product">
                <div class="cart__item-product">
                  <div class="cart__item-product-image">
                    <div class="image cart__item-product-image" style="--aspect-ratio: 1.499;">
                      <img src=${item.MainImg} sizes="110px" alt=${item.Name} loading="lazy" class="img-loaded" width="110" height="73"/>
                    </div>
                  </div>
                  <div class="cart__item-product-info">
                    <div class="cart__item-product-title">
                      <a href="${"/product/" + item.Handle}">${item.Name}</a>
                    </div>
                    <button class="cart-item__remove mt-2" data-id=${item.Id}>Remove</button>
                  </div>
                </div>
              </div>
              <div class="cart__table-col cart__table-price">
                <div class="cart__item-prices">
                  <div class="cart__item-discount-prices">
                    <p>
                      <span class="visually-hidden">Regular price</span>
                      <span class="money" data-product-price=${item.Price}>$${item.Price / 100} USD</span>
                    </p>
                  </div>
                </div>
                <div class="cart__quantity mt-2 md:hidden">
                  <div class="flex items-center justify-end">
                    <span class="mr-2 text-sm hidden sm:block">Qty</span>
                    <div class="cart-item__qty flex justify-between border border-color-border rounded">
                      <button class="cart-item__btn" data-id=${item.Id} data-qty-change="dec">-</button>
                      <input class="py-1 cart-item__qty_input w-9 text-center" type="number" name="quantity" id="quantity${index}-1" data-id=${item.Id} value=${item.Quantity} min="0">
                      <button class="cart-item__btn" data-id=${item.Id} data-qty-change="inc">+</button>
                    </div>
                  </div>
                </div>
              </div>
              <div class="cart__table-col cart__table-quantity hidden md:block">
                <div class="cart-item__qty flex justify-between border border-color-border rounded">
                  <button class="cart-item__btn" data-id=${item.Id} data-qty-change="dec">-</button>
                  <input class="py-1 cart-item__qty_input w-9 text-center" type="number" name="quantity" id="quantity${index}-2" data-id=${item.Id} value=${item.Quantity} min="0">
                  <button class="cart-item__btn" data-id=${item.Id} data-qty-change="inc">+</button>
                </div>
              </div>
              <div class="cart__table-col cart__table-subtotal text-right hidden md:block">
                <div>
                  <span class="font-bold cart-item__original_line_price">
                    <span class="money" data-product-price=${item.Price * item.Quantity}>$${(item.Price * item.Quantity) / 100} USD</span>
                  </span>
                </div>
              </div>
            </div>
            `,
                )
                .join("")}
          </div>
          `
            : `<div class="my-20 px-4"><h3 class="text-center text-xl">Your cart is currently empty. <a href="/product" class="border-b border-gray-800">Back to shopping</a></h3></div>`;

    const cartSubtotal = cartWrapper.querySelector(".cart-subtotal__price");
    cartSubtotal.innerHTML = `<span class="money" data-product-price=${currentCart.subtotal}></span>`;
    document.querySelectorAll(".currency-selector").forEach((selector) => {
        selector.dispatchEvent(new Event("change"));
    });

    const cartItemDecBtns = cartTable.querySelectorAll(
        ".cart-item__btn[data-qty-change='dec']",
    );
    const cartItemIncBtns = cartTable.querySelectorAll(
        ".cart-item__btn[data-qty-change='inc']",
    );
    const cartItemQtyInputs = cartTable.querySelectorAll(".cart-item__qty_input");
    const cartItemRemoveBtns = cartTable.querySelectorAll(".cart-item__remove");

    cartItemDecBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            if (currentCart.items[itemIndex].Quantity > 1) {
                currentCart.items[itemIndex].Quantity -= 1;
                currentCart.subtotal = currentCart.items.reduce(
                    (acc, item) => acc + item.Price * item.Quantity,
                    0,
                );
                sessionStorage.setItem("cart", JSON.stringify(currentCart));
                saveCart();
            } else {
                currentCart.subtotal = currentCart.items.reduce(
                    (acc, item) => acc + item.Price * item.Quantity,
                    0,
                );
                currentCart.items.splice(itemIndex, 1);
                sessionStorage.setItem("cart", JSON.stringify(currentCart));
                saveCart();
            }
        });
    });

    cartItemIncBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            currentCart.items[itemIndex].Quantity += 1;
            currentCart.subtotal = currentCart.items.reduce(
                (acc, item) => acc + item.Price * item.Quantity,
                0,
            );
            sessionStorage.setItem("cart", JSON.stringify(currentCart));
            saveCart();
        });
    });

    cartItemQtyInputs.forEach((input) => {
        input.addEventListener("change", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            const newQty = parseInt(e.target.value);
            if (newQty > 0) {
                currentCart.items[itemIndex].Quantity = newQty;
                currentCart.subtotal = currentCart.items.reduce(
                    (acc, item) => acc + item.Price * item.Quantity,
                    0,
                );
                sessionStorage.setItem("cart", JSON.stringify(currentCart));
                saveCart();
            } else {
                currentCart.items.splice(itemIndex, 1);
                currentCart.subtotal = currentCart.items.reduce(
                    (acc, item) => acc + item.Price * item.Quantity,
                    0,
                );
                sessionStorage.setItem("cart", JSON.stringify(currentCart));
                saveCart();
            }
        });
    });

    cartItemRemoveBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const id = e.target.getAttribute("data-id");
            const itemIndex = currentCart.items.findIndex(
                (item) => item.Id === parseInt(id),
            );
            currentCart.items.splice(itemIndex, 1);
            currentCart.subtotal = currentCart.items.reduce(
                (acc, item) => acc + item.Price * item.Quantity,
                0,
            );
            sessionStorage.setItem("cart", JSON.stringify(currentCart));
            saveCart();
        });
    });

    if (addToCart && currentCart.items.length > 0) {
        setTimeout(() => {
            cartBody.querySelector(".notification").classList.add("show");
        }, 500);
        setTimeout(() => {
            cartBody.querySelector(".notification").classList.remove("show");
        }, 3000);
        setTimeout(() => {
            cartBody.querySelector(".notification").remove();
        }, 3300);
    }
}

loadCart();

document.querySelector("button[name=checkout]").addEventListener("click", () => {
    if (sessionStorage.getItem("checkout"))
        localStorage.removeItem("checkout");
    window.location.href = "/checkout";
});

document.querySelector("button[name=paypal]").addEventListener("click", () => {
    if (sessionStorage.getItem("checkout"))
        localStorage.removeItem("checkout");
    window.location.href = "/checkout/?payment=paypal";
});
