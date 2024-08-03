// Event handlers for the menu on mobile
const menuButton_click = (e) => {
  const menuButton = e.target.closest(".menu-button");
  menuButton.classList.toggle("opened");
  menuButton.setAttribute(
    "aria-expanded",
    menuButton.classList.contains("opened"),
  );
  document.body.classList.toggle("menu-opened");
  document.documentElement.classList.toggle("prevent-scroll");
};

// Event handlers for opening the submenu on mobile
const subMenuOpen_click = (e) => {
  if (
    e.target.classList.contains("back-btn") ||
    e.target.parentElement.classList.contains("back-btn")
  )
    return;
  const menuContent = document.querySelector(".menu-content");
  if (!menuContent.classList.contains("sub-menu-open")) {
    const subMenuToggle = e.target.closest(".menu-link");
    const subMenu = subMenuToggle.querySelector(".sub-links");
    menuContent.classList.add("sub-menu-open");
    subMenu.classList.remove("hidden");
  }
};

// Event handlers for closing the submenu on mobile
const subMenuClose_click = (e) => {
  const menuContent = document.querySelector(".menu-content");
  if (menuContent.classList.contains("sub-menu-open")) {
    const subMenuToggle = e.target.closest(".menu-link");
    const subMenu = subMenuToggle.querySelector(".sub-links");
    menuContent.classList.remove("sub-menu-open");
    subMenu.classList.add("hidden");
  }
};

document
  .querySelector(".menu-button")
  .addEventListener("click", menuButton_click);
  

document.querySelectorAll(".menu-link").forEach((item) => {
  if (item.querySelector(".sub-links")) {
    item.addEventListener("click", subMenuOpen_click);
    item
      .querySelector(".back-btn")
      .addEventListener("click", subMenuClose_click);
  }
});

window.addEventListener("resize", () => {
  if (window.innerWidth > 1024) {
    document.body.classList.remove("menu-opened");
    document.documentElement.classList.remove("prevent-scroll");
    document.querySelector(".menu-button").classList.remove("opened");
    document.querySelector(".menu-content").classList.remove("sub-menu-open");
    document
      .querySelectorAll(".sub-links")
      .forEach((subMenu) => subMenu.classList.add("hidden"));
  }
});

//import { searchData } from "./searchData.js";

//const handleSearchResults = (e) => {
//  const searchResults = document.querySelector(".searchResults");
//  const searchResultsContent = document.querySelector(
//    ".searchResults__content",
//  );
//  const query = e.target.value.toLowerCase();
//  if (query === "") {
//    searchResults.classList.add("hidden");
//    searchResultsContent.innerHTML = "";
//    return;
//  }
//  searchResults.classList.remove("hidden");
//  const filteredResults = searchData.filter((item) =>
//    item.title.toLowerCase().includes(query),
//  );
//  filteredResults.sort((a, b) => {
//    let aTitle = a.title.toLowerCase();
//    let bTitle = b.title.toLowerCase();

//    if (aTitle.startsWith(query) && !bTitle.startsWith(query)) {
//      return -1;
//    } else if (!aTitle.startsWith(query) && bTitle.startsWith(query)) {
//      return 1;
//    } else {
//      return 0;
//    }
//  });
//  searchResultsContent.innerHTML =
//    filteredResults.length > 0
//      ? filteredResults
//        .slice(0, 6)
//        .map((item) => {
//          return `
//		<div class="searchResults__item">
//			<a href="${path + item.url}">
//				<div class="item__img">
//					<img src="${currentPath + item.img}" alt="${item.title}"/>
//				</div>
//				<div class="item__content">
//					<p class="item__title">${item.title}</p>
//					<div class="item__price">
//						<span class="money" data-product-price="${item.price}"><span>
//					</div>
//				</div>
//			</a>
//		</div>
//		`;
//        })
//        .join("")
//      : `<p>Sorry, nothing found for <strong>${e.target.value}</strong>.</p>`;

//  if (filteredResults.length > 0) {
//    if (!searchResults.querySelector(".searchResults__view-all")) {
//      const newNode = document.createElement("div");
//      newNode.classList.add("searchResults__view-all");
//      searchResults.appendChild(newNode);
//    }
//    const searchResultsViewAll = searchResults.querySelector(
//      ".searchResults__view-all",
//    );
//    searchResultsViewAll.innerHTML = `
//			<a href="${path}/pages/search-result/?s=${query}">View all results</a>
//		`;
//  } else {
//    searchResults.querySelector(".searchResults__view-all")?.remove();
//  }

//  updatePrice();
//};

//document
//  .querySelector(".desktop-header .search-input")
//  .addEventListener("input", handleSearchResults);

//document.addEventListener("click", (e) => {
//  if (!e.target.closest(".search-form")) {
//    document.querySelector(".searchResults").classList.add("hidden");
//  } else if (
//    e.target.closest(".search-input") !== null &&
//    e.target.closest(".search-input").value !== ""
//  ) {
//    document.querySelector(".searchResults").classList.remove("hidden");
//  }
//});

//const handleSearchResultsMobile = (e) => {
//  const searchResults = document.querySelector(".search-popup__results");
//  const query = e.target.value.toLowerCase();
//  if (query === "") {
//    searchResults.innerHTML = "";
//    return;
//  }
//  const filteredResults = searchData.filter((item) =>
//    item.title.toLowerCase().includes(query),
//  );
//  filteredResults.sort((a, b) => {
//    let aTitle = a.title.toLowerCase();
//    let bTitle = b.title.toLowerCase();

//    if (aTitle.startsWith(query) && !bTitle.startsWith(query)) {
//      return -1;
//    } else if (!aTitle.startsWith(query) && bTitle.startsWith(query)) {
//      return 1;
//    } else {
//      return 0;
//    }
//  });
//  searchResults.innerHTML =
//    filteredResults.length > 0
//      ? filteredResults
//        .slice(0, 8)
//        .map((item) => {
//          return `
//		<div class="searchResults__item">
//			<a href="${path + item.url}">
//				<div class="item__img">
//					<img src="${currentPath + item.img}" alt="${item.title}"/>
//				</div>
//				<div class="item__content">
//					<p class="item__title">${item.title}</p>
//					<div class="item__price">
//						<span class="money" data-product-price="${item.price}"><span>
//					</div>
//				</div>
//			</a>
//		</div>
//		`;
//        })
//        .join("")
//      : `<p>Sorry, nothing found for <strong>${e.target.value}</strong>.</p>`;

//  if (filteredResults.length > 0) {
//    if (!searchResults.querySelector(".searchResults__view-all")) {
//      const newNode = document.createElement("div");
//      newNode.classList.add("searchResults__view-all");
//      searchResults.appendChild(newNode);
//    }
//    const searchResultsViewAll = searchResults.querySelector(
//      ".searchResults__view-all",
//    );
//    searchResultsViewAll.innerHTML = `
//			<a href="${path}/pages/search-result/?s=${query}">View all results</a>
//		`;
//  } else {
//    searchResults.querySelector(".searchResults__view-all")?.remove();
//  }

//  updatePrice();
//};

//document.querySelector("button.search-popup").addEventListener("click", (e) => {
//  const searchPopup = document.createElement("div");
//  searchPopup.classList.add("search-popup-modal");
//  searchPopup.innerHTML = `
//		<div class="search-popup__content">
//			<div class="search-popup__header">
//				<button class="search-popup__close"></button>
//				<input
//					type="text"
//					name="search-popup"
//					class="search-popup__input"
//					placeholder="Search products..."
//				/>
//				<button class="search-popup__clear"></button>
//			</div>
//			<div class="search-popup__results"></div>
//		</div>
//	`;
//  document.body.appendChild(searchPopup);
//  document.querySelector(".search-popup__input").focus();
//  document.documentElement.classList.add("prevent-scroll");
//  document
//    .querySelector(".search-popup__input")
//    .addEventListener("input", handleSearchResultsMobile);

//  document
//    .querySelector(".search-popup__close")
//    .addEventListener("click", (e) => {
//      document.querySelector(".search-popup-modal").remove();
//      document.documentElement.classList.remove("prevent-scroll");
//    });
//  document
//    .querySelector(".search-popup__clear")
//    .addEventListener("click", (e) => {
//      document.querySelector(".search-popup__input").value = "";
//      document.querySelector(".search-popup__input").focus();
//      document.querySelector(".search-popup__results").innerHTML = "";
//    });
//});

//document
//  .querySelector("button.search-submit")
//  .addEventListener("click", (e) => {
//    e.preventDefault();
//    const searchInput = document.querySelector(".search-input");
//    window.location.href = `${path}/pages/search-result/?s=${searchInput.value}`;
//  });
