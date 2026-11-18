/**
 * DTGear — Interactive E-Commerce & Smart PC Builder Engine
 * Phong cách tối giản chuẩn Apple Store Minimalist
 */

document.addEventListener("DOMContentLoaded", function () {
    // Khởi tạo Lucide Icons nét mảnh đồng nhất
    if (window.lucide) {
        window.lucide.createIcons();
    }

    initSmartNavbar();
    initFloatingSidebar();
    initProductCatalogFilter();
    initBuildPcCalculator();
    initCartSystem();
});

/* ==========================================================================
   1. Smart Sticky Navbar
   ========================================================================== */
function initSmartNavbar() {
    const navbar = document.querySelector(".dt-navbar");
    if (!navbar) return;

    const onScroll = () => {
        if (window.scrollY > 16) {
            navbar.classList.add("dt-navbar-scrolled");
        } else {
            navbar.classList.remove("dt-navbar-scrolled");
        }
    };
    window.addEventListener("scroll", onScroll, { passive: true });
    onScroll();

    // Phím tắt "/" mở nhanh Spotlight Search trên Desktop
    const spotlightInput = document.querySelector(".dt-spotlight-input");
    if (spotlightInput) {
        document.addEventListener("keydown", (e) => {
            if (e.key === "/" && !["INPUT", "TEXTAREA", "SELECT"].includes(document.activeElement?.tagName || "")) {
                e.preventDefault();
                spotlightInput.focus();
            }
        });
    }
}

/* ==========================================================================
   1.1. Apple Studio Floating Glass Sidebar (Thu gọn 68px <-> Mở rộng 240px mượt mà)
   ========================================================================== */
function initFloatingSidebar() {
    const sidebar = document.getElementById("dtFloatingSidebar");
    const navContainer = document.getElementById("dtSidebarNav");
    const hoverPill = document.getElementById("dtSidebarHoverPill");
    const topToggleBtn = document.getElementById("dtDesktopSidebarToggleBtn");
    const innerToggleBtn = document.getElementById("dtSidebarInnerToggleBtn");

    if (!sidebar) return;

    function setExpanded(expanded) {
        sidebar.classList.toggle("is-expanded", expanded);
        if (topToggleBtn) {
            topToggleBtn.classList.toggle("is-active", expanded);
            topToggleBtn.setAttribute("aria-expanded", expanded ? "true" : "false");
        }
        if (innerToggleBtn) {
            innerToggleBtn.setAttribute("aria-expanded", expanded ? "true" : "false");
            const tooltip = innerToggleBtn.querySelector(".dt-sidebar-tooltip");
            if (tooltip) {
                tooltip.textContent = expanded ? "Thu gọn menu" : "Mở rộng menu";
            }
        }
    }

    function toggleSidebar(e) {
        if (e) e.stopPropagation();
        const isExpanded = sidebar.classList.contains("is-expanded");
        setExpanded(!isExpanded);
    }

    if (topToggleBtn) {
        topToggleBtn.addEventListener("click", toggleSidebar);
    }
    if (innerToggleBtn) {
        innerToggleBtn.addEventListener("click", toggleSidebar);
    }

    // Tự động mở rộng mượt mà khi rê chuột vào Sidebar và thu gọn khi rời chuột
    sidebar.addEventListener("mouseenter", () => {
        setExpanded(true);
    });
    sidebar.addEventListener("mouseleave", () => {
        setExpanded(false);
    });

    // Tự động thu gọn nhẹ nhàng khi bấm ra ngoài hoặc nhấn phím Escape
    document.addEventListener("click", (e) => {
        if (!sidebar.classList.contains("is-expanded")) return;
        const target = e.target;
        if (!sidebar.contains(target) && (!topToggleBtn || !topToggleBtn.contains(target))) {
            setExpanded(false);
        }
    });

    document.addEventListener("keydown", (e) => {
        if (e.key === "Escape" && sidebar.classList.contains("is-expanded")) {
            setExpanded(false);
        }
    });

    // Hiệu ứng khối nền Hover (#F5F5F7) trượt mượt theo con trỏ
    if (navContainer && hoverPill) {
        const items = navContainer.querySelectorAll(".dt-sidebar-item");
        items.forEach((item) => {
            item.addEventListener("mouseenter", () => {
                if (item.classList.contains("active")) {
                    hoverPill.style.opacity = "0";
                    return;
                }
                hoverPill.style.height = `${item.offsetHeight}px`;
                hoverPill.style.transform = `translateY(${item.offsetTop}px)`;
                hoverPill.style.opacity = "1";
            });
        });

        navContainer.addEventListener("mouseleave", () => {
            hoverPill.style.opacity = "0";
        });
    }
}

/* ==========================================================================
   2. Bộ lọc Danh mục, Nhu cầu, Khoảng giá & Tìm kiếm nhanh
   ========================================================================== */
function initProductCatalogFilter() {
    const productCols = document.querySelectorAll(".dt-product-col");
    if (!productCols.length) return;

    const categoryButtons = document.querySelectorAll("[data-filter-cat]");
    const sidebarItems = document.querySelectorAll(".dt-sidebar-item[data-sidebar-key]");
    const priceSelect = document.getElementById("dtPriceFilter");
    const subCatSelect = document.getElementById("dtSubCatFilter");
    const searchInputs = document.querySelectorAll(".js-dt-search-input");
    const needCards = document.querySelectorAll(".js-need-card");
    const countLabel = document.getElementById("dtProductCount");
    const emptyState = document.getElementById("dtEmptyCatalogState");

    let activeCategory = "all";
    let activeNeed = "all";
    let activeSubCat = "all";
    let activePrice = "all";
    let searchKeyword = "";

    function syncSidebarActiveState(cat) {
        const targetKey = !cat || cat === "all" ? "home" : cat;
        sidebarItems.forEach((item) => {
            const key = item.dataset.sidebarKey || "";
            if (key === "Cart" || key === "BuildPc" || key === "Account") return;
            item.classList.toggle("active", key.toLowerCase() === targetKey.toLowerCase());
        });
    }

    function applyFilters() {
        let visibleCount = 0;

        productCols.forEach((col) => {
            const cat = (col.dataset.category || "").toLowerCase();
            const subcat = (col.dataset.subcategory || "").toLowerCase();
            const need = (col.dataset.need || "").toLowerCase();
            const name = (col.dataset.name || "").toLowerCase();
            const specs = (col.dataset.specs || "").toLowerCase();
            const price = parseFloat(col.dataset.price || "0");

            const matchCat = activeCategory === "all" || cat === activeCategory.toLowerCase();
            const matchNeed = activeNeed === "all" || need === activeNeed.toLowerCase();
            const matchSub = activeSubCat === "all" || subcat === activeSubCat.toLowerCase();

            let matchPrice = true;
            if (activePrice === "under1m") matchPrice = price < 1000000;
            else if (activePrice === "1mto10m") matchPrice = price >= 1000000 && price <= 10000000;
            else if (activePrice === "10mto25m") matchPrice = price > 10000000 && price <= 25000000;
            else if (activePrice === "over25m") matchPrice = price > 25000000;

            const kw = searchKeyword.trim().toLowerCase();
            const matchSearch = !kw || name.includes(kw) || subcat.includes(kw) || specs.includes(kw) || cat.includes(kw);

            if (matchCat && matchNeed && matchSub && matchPrice && matchSearch) {
                col.classList.remove("d-none");
                visibleCount++;
            } else {
                col.classList.add("d-none");
            }
        });

        if (countLabel) {
            countLabel.textContent = visibleCount.toString();
        }
        if (emptyState) {
            emptyState.classList.toggle("d-none", visibleCount > 0);
        }
        syncSidebarActiveState(activeCategory);
    }

    // Nút lọc nhóm chính (Tất cả, Laptop, PC, Linh kiện, Phụ kiện)
    categoryButtons.forEach((btn) => {
        btn.addEventListener("click", () => {
            categoryButtons.forEach((b) => b.classList.remove("active"));
            btn.classList.add("active");
            activeCategory = btn.dataset.filterCat || "all";
            activeNeed = "all";
            needCards.forEach((c) => c.classList.remove("active"));
            applyFilters();
        });
    });

    // Thẻ phân loại theo Nhu cầu & Ngân sách (4 nhóm)
    needCards.forEach((card) => {
        card.addEventListener("click", () => {
            const selectedNeed = card.dataset.needGroup || "all";
            if (activeNeed === selectedNeed) {
                activeNeed = "all";
                card.classList.remove("active");
            } else {
                needCards.forEach((c) => c.classList.remove("active"));
                card.classList.add("active");
                activeNeed = selectedNeed;
                activeCategory = "all";
                categoryButtons.forEach((b) => {
                    b.classList.toggle("active", b.dataset.filterCat === "all");
                });
            }
            applyFilters();
            const catalogSection = document.getElementById("dt-catalog-section");
            if (catalogSection) {
                catalogSection.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
    });

    // Lọc theo danh mục con (CPU, VGA, RAM, Tai nghe, USB...)
    if (subCatSelect) {
        subCatSelect.addEventListener("change", () => {
            activeSubCat = subCatSelect.value;
            applyFilters();
        });
    }

    // Lọc theo khoảng giá
    if (priceSelect) {
        priceSelect.addEventListener("change", () => {
            activePrice = priceSelect.value;
            applyFilters();
        });
    }

    // Thanh tìm kiếm nhanh trên Navbar & trong danh mục
    searchInputs.forEach((input) => {
        input.addEventListener("input", (e) => {
            searchKeyword = e.target.value || "";
            searchInputs.forEach((other) => {
                if (other !== e.target) other.value = searchKeyword;
            });
            applyFilters();
        });

        input.addEventListener("keydown", (e) => {
            if (e.key === "Enter") {
                e.preventDefault();
                const catalogSection = document.getElementById("dt-catalog-section");
                if (catalogSection) {
                    catalogSection.scrollIntoView({ behavior: "smooth", block: "start" });
                }
            }
        });
    });

    // Nút liên kết nhanh từ Navbar dropdown, Floating Sidebar & Mobile Drawer
    document.querySelectorAll(".js-nav-filter").forEach((link) => {
        link.addEventListener("click", (e) => {
            const cat = link.dataset.cat || "all";
            const sub = link.dataset.subcat || "all";
            const catalogSection = document.getElementById("dt-catalog-section");
            if (!catalogSection) return; // Nếu đang ở trang khác thì để chuyển hướng bình thường

            e.preventDefault();
            activeCategory = cat;
            activeSubCat = sub;
            activeNeed = "all";

            categoryButtons.forEach((b) => {
                b.classList.toggle("active", (b.dataset.filterCat || "all").toLowerCase() === cat.toLowerCase());
            });
            if (subCatSelect) {
                subCatSelect.value = sub === "all" ? "all" : sub;
            }
            applyFilters();

            if (cat === "all" && sub === "all" && link.dataset.sidebarKey === "home") {
                window.scrollTo({ top: 0, behavior: "smooth" });
            } else {
                catalogSection.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
    });

    // Nút đặt lại bộ lọc
    const resetBtn = document.getElementById("dtResetFilterBtn");
    if (resetBtn) {
        resetBtn.addEventListener("click", () => {
            activeCategory = "all";
            activeNeed = "all";
            activeSubCat = "all";
            activePrice = "all";
            searchKeyword = "";
            if (subCatSelect) subCatSelect.value = "all";
            if (priceSelect) priceSelect.value = "all";
            searchInputs.forEach((inp) => (inp.value = ""));
            needCards.forEach((c) => c.classList.remove("active"));
            categoryButtons.forEach((b) => b.classList.toggle("active", b.dataset.filterCat === "all"));
            applyFilters();
        });
    }
}

/* ==========================================================================
   3. Công cụ "Build PC cùng DTGear" (Mặc định 0 ₫, Tự động cộng tiền & Đồng bộ Form)
   ========================================================================== */
function initBuildPcCalculator() {
    const builderRoot = document.getElementById("dtPcBuilderApp");
    if (!builderRoot) return;

    const partSelects = builderRoot.querySelectorAll(".js-pc-part-select");
    const accessoryCheckboxes = builderRoot.querySelectorAll(".js-pc-acc-checkbox");
    const presetButtons = builderRoot.querySelectorAll(".js-pc-preset-btn");
    const resetBuilderBtn = document.getElementById("dtResetBuilderBtn");

    const coreSubtotalEl = document.getElementById("dtCorePcSubtotal");
    const accSubtotalEl = document.getElementById("dtAccPcSubtotal");
    const grandTotalEl = document.getElementById("dtGrandPcTotal");
    const partCountBadgeEl = document.getElementById("dtSelectedPartCountBadge");
    const compatStatusBox = document.getElementById("dtCompatStatusBox");
    const compatTextEl = document.getElementById("dtCompatStatusText");
    const wattageTextEl = document.getElementById("dtWattageText");
    const selectedListEl = document.getElementById("dtSelectedPartsPreview");
    const applyToOrderBtn = document.getElementById("dtApplyBuildToOrderBtn");
    const addBuildToCartBtn = document.getElementById("dtAddBuildToCartBtn");

    function formatVnd(amount) {
        return Math.round(amount).toLocaleString("vi-VN") + " ₫";
    }

    function syncBuildToOrderForm(summaryText, totalAmount, forceSync) {
        const formSummaryEl = document.getElementById("OrderForm_ConfigurationSummary");
        const formTotalEl = document.getElementById("OrderForm_TotalEstimatedAmount");
        const formTotalPreviewEl = document.getElementById("dtFormTotalDisplay");

        // Luôn đồng bộ tức thì trên trang Build PC hoặc khi người dùng thao tác chọn linh kiện
        const isBuildPcPage = window.location.pathname.toLowerCase().includes("buildpc");
        if (!isBuildPcPage && !forceSync) return;

        if (formSummaryEl) {
            formSummaryEl.value = totalAmount > 0 ? summaryText : "";
        }
        if (formTotalEl) {
            formTotalEl.value = totalAmount.toString();
        }
        if (formTotalPreviewEl) {
            formTotalPreviewEl.textContent = formatVnd(totalAmount);
        }
    }

    function recalculateBuild(syncForm = false) {
        let coreSubtotal = 0;
        let accSubtotal = 0;
        let selectedPartsCount = 0;
        let estimatedWattage = 0;
        let psuCapacity = 0;

        let cpuSocket = "";
        let mainSocket = "";
        let mainRamType = "";
        let ramType = "";

        const selectedSummaryLines = [];

        partSelects.forEach((select) => {
            const opt = select.options[select.selectedIndex];
            const rowPriceEl = document.getElementById(`price-${select.id}`);
            const rowContainer = select.closest(".dt-part-row");

            if (!opt || !opt.value) {
                if (rowPriceEl) rowPriceEl.textContent = "0 ₫";
                if (rowContainer) rowContainer.classList.remove("dt-part-row-selected");
                return;
            }

            const partType = select.dataset.partType || "";
            const name = opt.dataset.name || opt.textContent.trim();
            const price = parseFloat(opt.dataset.price || "0");
            const watt = parseInt(opt.dataset.wattage || "0", 10);
            const socket = opt.dataset.socket || "";
            const rType = opt.dataset.ramtype || "";

            selectedPartsCount++;
            coreSubtotal += price;
            selectedSummaryLines.push(`• [${partType}] ${name} (${formatVnd(price)})`);

            if (rowPriceEl) {
                rowPriceEl.textContent = formatVnd(price);
            }
            if (rowContainer) {
                rowContainer.classList.add("dt-part-row-selected");
            }

            if (partType === "CPU") {
                cpuSocket = socket;
                estimatedWattage += watt;
            } else if (partType === "Mainboard") {
                mainSocket = socket;
                mainRamType = rType;
                estimatedWattage += watt;
            } else if (partType === "RAM") {
                ramType = rType;
                estimatedWattage += watt;
            } else if (partType === "VGA") {
                estimatedWattage += watt;
            } else if (partType === "SSD") {
                estimatedWattage += watt;
            } else if (partType === "PSU") {
                psuCapacity = watt;
            }
        });

        if (selectedPartsCount > 0 && estimatedWattage > 0) {
            estimatedWattage += 45; // Công suất quạt tản nhiệt & ngoại vi khi có linh kiện
        }

        const selectedAccessories = [];
        accessoryCheckboxes.forEach((cb) => {
            const card = cb.closest(".dt-accessory-check-card");
            if (cb.checked) {
                const name = cb.dataset.name || "";
                const price = parseFloat(cb.dataset.price || "0");
                selectedPartsCount++;
                accSubtotal += price;
                selectedAccessories.push(`+ [Phụ kiện] ${name} (${formatVnd(price)})`);
                if (card) card.classList.add("checked");
            } else {
                if (card) card.classList.remove("checked");
            }
        });

        const grandTotal = coreSubtotal + accSubtotal;

        if (coreSubtotalEl) coreSubtotalEl.textContent = formatVnd(coreSubtotal);
        if (accSubtotalEl) accSubtotalEl.textContent = formatVnd(accSubtotal);
        if (grandTotalEl) grandTotalEl.textContent = formatVnd(grandTotal);
        if (partCountBadgeEl) partCountBadgeEl.textContent = `${selectedPartsCount} món`;

        if (wattageTextEl) {
            wattageTextEl.textContent = `Công suất tiêu thụ ước tính: ~${estimatedWattage}W / Nguồn đã chọn: ${psuCapacity}W`;
        }

        // Kiểm tra tương thích phần cứng thông minh
        const issues = [];
        if (cpuSocket && mainSocket && cpuSocket !== mainSocket) {
            issues.push(`CPU (${cpuSocket}) chưa khớp Socket với Mainboard (${mainSocket}).`);
        }
        if (mainRamType && ramType && mainRamType !== ramType) {
            issues.push(`Mainboard hỗ trợ chuẩn ${mainRamType} nhưng RAM đang chọn là ${ramType}.`);
        }
        if (psuCapacity > 0 && psuCapacity < estimatedWattage + 60) {
            issues.push(`Nguồn ${psuCapacity}W hơi sát mức tải ~${estimatedWattage}W, khuyến nghị nâng nguồn cao hơn.`);
        }

        if (compatStatusBox && compatTextEl) {
            if (selectedPartsCount === 0) {
                compatStatusBox.className = "dt-compat-badge dt-compat-ok mb-3";
                compatTextEl.textContent = "Hãy chọn linh kiện để kiểm tra tương thích & tính tổng tự động";
            } else if (issues.length === 0) {
                compatStatusBox.className = "dt-compat-badge dt-compat-ok mb-3";
                compatTextEl.textContent = "Tương thích 100% — Các linh kiện đã chọn đồng bộ chuẩn mực";
            } else {
                compatStatusBox.className = "dt-compat-badge dt-compat-warn mb-3";
                compatTextEl.textContent = "Lưu ý tương thích: " + issues.join(" ");
            }
        }

        if (selectedListEl) {
            if (selectedSummaryLines.length === 0 && selectedAccessories.length === 0) {
                selectedListEl.innerHTML = `<div class="text-secondary">Chưa có linh kiện nào được chọn (Tổng: 0 ₫).</div>`;
            } else {
                const allLines = [...selectedSummaryLines, ...selectedAccessories];
                const previewHtml = allLines
                    .slice(0, 6)
                    .map((line) => `<div class="small text-dark mb-1 text-truncate">${line}</div>`)
                    .join("");
                const extraCount = allLines.length - 6;
                selectedListEl.innerHTML =
                    previewHtml +
                    (extraCount > 0
                        ? `<div class="small fw-semibold text-primary mt-1">+ ${extraCount} thiết bị khác trong cấu hình</div>`
                        : "");
            }
        }

        const fullSummary =
            grandTotal > 0
                ? [
                      "=== CẤU HÌNH BUILD PC CÙNG DTGEAR ===",
                      ...selectedSummaryLines,
                      ...(selectedAccessories.length ? ["--- Phụ kiện & Gear đi kèm ---", ...selectedAccessories] : []),
                      `=> TỔNG CHI PHÍ CẤU HÌNH: ${formatVnd(grandTotal)}`
                  ].join("\n")
                : "";

        builderRoot.dataset.FullSummary = fullSummary;
        builderRoot.dataset.TotalAmount = grandTotal.toString();

        syncBuildToOrderForm(fullSummary, grandTotal, syncForm);
    }

    partSelects.forEach((select) => {
        select.addEventListener("change", () => {
            presetButtons.forEach((b) => b.classList.remove("active"));
            recalculateBuild(true);
        });
    });

    accessoryCheckboxes.forEach((cb) => {
        cb.addEventListener("change", () => {
            recalculateBuild(true);
        });
    });

    // Áp dụng cấu hình mẫu (Preset)
    presetButtons.forEach((btn) => {
        btn.addEventListener("click", () => {
            presetButtons.forEach((b) => b.classList.remove("active"));
            btn.classList.add("active");

            const map = {
                CPU: btn.dataset.cpu,
                Mainboard: btn.dataset.mainboard,
                RAM: btn.dataset.ram,
                VGA: btn.dataset.vga,
                SSD: btn.dataset.ssd,
                PSU: btn.dataset.psu,
                Case: btn.dataset.case,
                Monitor: btn.dataset.monitor
            };

            partSelects.forEach((select) => {
                const pType = select.dataset.partType;
                if (pType && map[pType]) {
                    select.value = map[pType];
                }
            });

            const accList = (btn.dataset.accessories || "").split(",").filter(Boolean);
            accessoryCheckboxes.forEach((cb) => {
                cb.checked = accList.includes(cb.value);
            });

            recalculateBuild(true);
        });
    });

    // Nút Làm mới (Đưa toàn bộ về 0 ₫)
    if (resetBuilderBtn) {
        resetBuilderBtn.addEventListener("click", () => {
            presetButtons.forEach((b) => b.classList.remove("active"));
            partSelects.forEach((select) => {
                select.value = "";
            });
            accessoryCheckboxes.forEach((cb) => {
                cb.checked = false;
            });
            recalculateBuild(true);
        });
    }

    // Nút chuyển cấu hình Build PC xuống Form đặt hàng / nhận báo giá
    if (applyToOrderBtn) {
        applyToOrderBtn.addEventListener("click", () => {
            recalculateBuild(true);
            const orderSection = document.getElementById("dt-order-section");
            if (orderSection) {
                orderSection.scrollIntoView({ behavior: "smooth", block: "start" });
                const nameInput = document.getElementById("OrderForm_CustomerName");
                if (nameInput) {
                    setTimeout(() => nameInput.focus(), 450);
                }
            }
        });
    }

    // Nút thêm toàn bộ linh kiện đã chọn trong Build PC vào Giỏ hàng
    if (addBuildToCartBtn) {
        addBuildToCartBtn.addEventListener("click", () => {
            const selectedItems = [];
            partSelects.forEach((select) => {
                const opt = select.options[select.selectedIndex];
                if (!opt || !opt.value) return;
                selectedItems.push({
                    id: opt.value,
                    name: opt.dataset.name || opt.textContent.trim(),
                    subCategory: select.dataset.partType || "Linh kiện PC",
                    specs: opt.dataset.socket ? `Socket ${opt.dataset.socket}` : "Linh kiện Build PC",
                    img: (opt.dataset.img || `/images/products/${opt.value}.jpg`).replace(/\.svg$/i, ".jpg"),
                    price: parseFloat(opt.dataset.price || "0")
                });
            });
            accessoryCheckboxes.forEach((cb) => {
                if (!cb.checked) return;
                selectedItems.push({
                    id: cb.value,
                    name: cb.dataset.name || "Phụ kiện DTGear",
                    subCategory: "Phụ kiện Build PC",
                    specs: "Phụ kiện mua kèm cấu hình",
                    img: (cb.dataset.img || `/images/products/${cb.value}.jpg`).replace(/\.svg$/i, ".jpg"),
                    price: parseFloat(cb.dataset.price || "0")
                });
            });

            if (selectedItems.length === 0) {
                alert("Bạn chưa chọn linh kiện nào. Hãy chọn ít nhất 1 linh kiện hoặc bấm Gợi ý cấu hình mẫu!");
                return;
            }

            if (window.dtGearAddMultipleToCart) {
                window.dtGearAddMultipleToCart(selectedItems);
            }
        });
    }

    // Khởi tạo lần đầu ở trạng thái 0 ₫ chuẩn xác
    recalculateBuild(false);
}

/* ==========================================================================
   4. Hệ thống Giỏ hàng & Báo giá Chi tiết (Có ảnh thu nhỏ, Tăng/Giảm số lượng, Xóa)
   ========================================================================== */
function initCartSystem() {
    const STORAGE_KEY = "dtgear_cart_items_v2";
    const cartCountBadges = document.querySelectorAll(".js-dt-cart-count");
    const cartModalCountText = document.getElementById("dtCartModalCountText");
    const cartItemsContainer = document.getElementById("dtCartItemsContainer");
    const cartTotalEl = document.getElementById("dtCartModalTotal");
    const checkoutCartBtn = document.getElementById("dtCheckoutCartToFormBtn");
    const clearCartBtn = document.getElementById("dtClearCartBtn");

    function loadCart() {
        try {
            const raw = localStorage.getItem(STORAGE_KEY);
            if (!raw) return [];
            const parsed = JSON.parse(raw);
            return Array.isArray(parsed)
                ? parsed.map((item) => ({
                      ...item,
                      img: (item.img || `/images/products/${item.id}.jpg`).replace(/\.svg$/i, ".jpg")
                  }))
                : [];
        } catch {
            return [];
        }
    }

    function saveCart(items) {
        try {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
        } catch {
            // ignore storage errors
        }
        renderCartUI(items);
    }

    function formatVnd(amount) {
        return Math.round(amount).toLocaleString("vi-VN") + " ₫";
    }

    function renderCartUI(items) {
        const totalQty = items.reduce((sum, item) => sum + item.qty, 0);
        const totalAmount = items.reduce((sum, item) => sum + item.price * item.qty, 0);

        cartCountBadges.forEach((badge) => {
            badge.textContent = totalQty.toString();
        });
        if (cartModalCountText) {
            cartModalCountText.textContent = totalQty.toString();
        }

        if (!cartItemsContainer) return;

        if (items.length === 0) {
            cartItemsContainer.innerHTML = `
                <div class="text-center py-5">
                    <div class="mb-3 text-secondary opacity-50">
                        <i data-lucide="shopping-bag" style="width: 44px; height: 44px;"></i>
                    </div>
                    <h6 class="fw-bold text-dark mb-1">Giỏ hàng của bạn đang trống</h6>
                    <p class="small text-secondary mb-0">Hãy chọn Laptop, Linh kiện PC hoặc Phụ kiện (Tai nghe, USB, Chuột...) để xem chi tiết báo giá tại đây.</p>
                </div>`;
            if (cartTotalEl) cartTotalEl.textContent = "0 ₫";
            if (window.lucide) window.lucide.createIcons();
            return;
        }

        cartItemsContainer.innerHTML = items
            .map((item, idx) => {
                const imgSrc = (item.img || `/images/products/${item.id}.jpg`).replace(/\.svg$/i, ".jpg");
                const specsText = item.specs || item.subCategory || "Sản phẩm chính hãng DTGear";
                return `
                <div class="dt-cart-item-row py-3 border-bottom">
                    <div class="dt-cart-item-main">
                        <div class="dt-cart-thumb-wrap flex-shrink-0">
                            <img src="${imgSrc}" alt="${item.name}" class="dt-cart-thumb-img" onerror="window.handleProductImageError(this, '${item.subCategory}')" />
                        </div>
                        <div class="dt-cart-item-info">
                            <div class="dt-subcat-label mb-0.5">${item.subCategory}</div>
                            <div class="dt-cart-item-title" title="${item.name}">${item.name}</div>
                            <div class="dt-cart-item-specs">${specsText}</div>
                            <div class="small fw-semibold text-secondary mt-1">Đơn giá: ${formatVnd(item.price)}</div>
                        </div>
                    </div>

                    <div class="dt-cart-item-actions">
                        <!-- Bộ nút Tăng / Giảm số lượng -->
                        <div class="dt-qty-stepper d-inline-flex align-items-center border rounded-pill bg-light px-1.5 py-0.5 flex-shrink-0">
                            <button type="button" class="btn btn-sm border-0 px-2 py-0.5 fw-bold js-cart-qty-minus" data-index="${idx}" aria-label="Giảm số lượng">−</button>
                            <span class="px-2 small fw-bold text-dark">${item.qty}</span>
                            <button type="button" class="btn btn-sm border-0 px-2 py-0.5 fw-bold js-cart-qty-plus" data-index="${idx}" aria-label="Tăng số lượng">+</button>
                        </div>

                        <div class="dt-cart-item-subtotal text-end flex-shrink-0">
                            <div class="fw-bold text-primary dt-price">${formatVnd(item.price * item.qty)}</div>
                        </div>

                        <button type="button" class="btn btn-sm btn-outline-danger rounded-pill px-3 py-1 flex-shrink-0 js-remove-cart-item" data-index="${idx}" title="Xóa khỏi giỏ">
                            Xóa
                        </button>
                    </div>
                </div>`;
            })
            .join("");

        if (cartTotalEl) {
            cartTotalEl.textContent = formatVnd(totalAmount);
        }

        // Sự kiện giảm số lượng
        cartItemsContainer.querySelectorAll(".js-cart-qty-minus").forEach((btn) => {
            btn.addEventListener("click", () => {
                const idx = parseInt(btn.dataset.index || "0", 10);
                const current = loadCart();
                if (!current[idx]) return;
                if (current[idx].qty > 1) {
                    current[idx].qty -= 1;
                } else {
                    current.splice(idx, 1);
                }
                saveCart(current);
            });
        });

        // Sự kiện tăng số lượng
        cartItemsContainer.querySelectorAll(".js-cart-qty-plus").forEach((btn) => {
            btn.addEventListener("click", () => {
                const idx = parseInt(btn.dataset.index || "0", 10);
                const current = loadCart();
                if (!current[idx]) return;
                current[idx].qty += 1;
                saveCart(current);
            });
        });

        // Sự kiện xóa sản phẩm
        cartItemsContainer.querySelectorAll(".js-remove-cart-item").forEach((btn) => {
            btn.addEventListener("click", () => {
                const idx = parseInt(btn.dataset.index || "0", 10);
                const current = loadCart();
                current.splice(idx, 1);
                saveCart(current);
            });
        });
    }

    // Cho phép công cụ Build PC thêm hàng loạt linh kiện vào Giỏ hàng
    window.dtGearAddMultipleToCart = function (newItems) {
        const items = loadCart();
        newItems.forEach((ni) => {
            const existing = items.find((x) => x.id === ni.id);
            if (existing) {
                existing.qty += 1;
            } else {
                items.push({ ...ni, qty: 1 });
            }
        });
        saveCart(items);
        const modalEl = document.getElementById("dtCartModal");
        if (modalEl && window.bootstrap) {
            const modalInstance = window.bootstrap.Modal.getOrCreateInstance(modalEl);
            modalInstance.show();
        }
    };

    function populateOrderFormFromCart(items) {
        if (!items || !items.length) return;
        const total = items.reduce((s, i) => s + i.price * i.qty, 0);
        const lines = [
            "=== DANH SÁCH SẢN PHẨM ĐẶT MUA TẠI DTGEAR ===",
            ...items.map((i) => `• [${i.subCategory}] ${i.name} (SL: ${i.qty}) = ${formatVnd(i.price * i.qty)}`),
            `=> TỔNG THANH TOÁN: ${formatVnd(total)}`
        ].join("\n");

        const formSummaryEl = document.getElementById("OrderForm_ConfigurationSummary");
        const formTotalEl = document.getElementById("OrderForm_TotalEstimatedAmount");
        const formTotalPreviewEl = document.getElementById("dtFormTotalDisplay");

        if (formSummaryEl) formSummaryEl.value = lines;
        if (formTotalEl) formTotalEl.value = total.toString();
        if (formTotalPreviewEl) formTotalPreviewEl.textContent = formatVnd(total);
    }

    // Nút "Mua ngay" trên thẻ sản phẩm hoặc trang Chi tiết sản phẩm -> Thêm vào giỏ & điền thẳng xuống Form đặt hàng
    document.querySelectorAll(".js-buy-now-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            const id = btn.dataset.id || "";
            const name = btn.dataset.name || "";
            const subCategory = btn.dataset.subcat || "";
            const specs = btn.dataset.specs || "";
            const img = (btn.dataset.img || `/images/products/${id}.jpg`).replace(/\.svg$/i, ".jpg");
            const price = parseFloat(btn.dataset.price || "0");

            const items = loadCart();
            const existing = items.find((x) => x.id === id);
            if (existing) {
                existing.qty += 1;
            } else if (id) {
                items.push({ id, name, subCategory, specs, img, price, qty: 1 });
            }
            saveCart(items);

            const orderSection = document.getElementById("dt-order-section");
            if (orderSection) {
                populateOrderFormFromCart(items);
                orderSection.scrollIntoView({ behavior: "smooth", block: "start" });
            } else {
                try {
                    sessionStorage.setItem("dtgear_auto_checkout", "1");
                } catch {}
                window.location.href = "/#dt-order-section";
            }
        });
    });

    // Nút "Thêm vào giỏ" trên thẻ sản phẩm & trang Chi tiết sản phẩm
    document.querySelectorAll(".js-add-cart-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            const id = btn.dataset.id || "";
            const name = btn.dataset.name || "";
            const subCategory = btn.dataset.subcat || "";
            const specs = btn.dataset.specs || "";
            const img = (btn.dataset.img || `/images/products/${id}.jpg`).replace(/\.svg$/i, ".jpg");
            const price = parseFloat(btn.dataset.price || "0");

            const items = loadCart();
            const existing = items.find((x) => x.id === id);
            if (existing) {
                existing.qty += 1;
                existing.img = img;
                existing.specs = specs;
            } else {
                items.push({ id, name, subCategory, specs, img, price, qty: 1 });
            }
            saveCart(items);

            // Hiệu ứng phản hồi nhẹ nhàng trên nút bấm
            const originalHtml = btn.innerHTML;
            btn.innerHTML = `<span class="dt-icon-inline">✓ Đã thêm vào giỏ</span>`;
            btn.classList.add("active");
            setTimeout(() => {
                btn.innerHTML = originalHtml;
                btn.classList.remove("active");
                if (window.lucide) window.lucide.createIcons();
            }, 950);
        });
    });

    // Chuyển toàn bộ Giỏ hàng xuống Form đặt hàng / nhận báo giá
    if (checkoutCartBtn) {
        checkoutCartBtn.addEventListener("click", () => {
            const items = loadCart();
            if (!items.length) return;

            populateOrderFormFromCart(items);

            const orderSection = document.getElementById("dt-order-section");
            if (orderSection) {
                orderSection.scrollIntoView({ behavior: "smooth", block: "start" });
            } else {
                try {
                    sessionStorage.setItem("dtgear_auto_checkout", "1");
                } catch {}
                window.location.href = "/#dt-order-section";
            }
        });
    }

    if (clearCartBtn) {
        clearCartBtn.addEventListener("click", () => {
            saveCart([]);
        });
    }

    const initialItems = loadCart();
    renderCartUI(initialItems);

    try {
        if (sessionStorage.getItem("dtgear_auto_checkout") === "1") {
            sessionStorage.removeItem("dtgear_auto_checkout");
            populateOrderFormFromCart(initialItems);
        }
    } catch {}
}

/* ==========================================================================
   5. Fallback Ảnh Sản Phẩm Chuẩn Apple Studio (Không bao giờ lỗi ảnh gãy)
   ========================================================================== */
window.handleProductImageError = function (imgEl, label) {
    imgEl.onerror = null;
    const safeLabel = (label || "DTGear Hardware").replace(/["<>]/g, "");
    const svg = `<svg xmlns="http://www.w3.org/2000/svg" width="400" height="400" viewBox="0 0 400 400">
        <rect width="400" height="400" fill="#F5F5F7" rx="20"/>
        <rect x="90" y="95" width="220" height="150" rx="14" fill="#FFFFFF" stroke="#1D1D1F" stroke-width="2.5"/>
        <line x1="160" y1="245" x2="150" y2="280" stroke="#1D1D1F" stroke-width="2.5"/>
        <line x1="240" y1="245" x2="250" y2="280" stroke="#1D1D1F" stroke-width="2.5"/>
        <line x1="130" y1="280" x2="270" y2="280" stroke="#1D1D1F" stroke-width="2.5" stroke-linecap="round"/>
        <text x="200" y="172" text-anchor="middle" font-family="Inter, sans-serif" font-size="16" font-weight="700" fill="#1D1D1F">DTGear Studio</text>
        <text x="200" y="325" text-anchor="middle" font-family="Inter, sans-serif" font-size="13" font-weight="500" fill="#86868B">${safeLabel}</text>
    </svg>`;
    imgEl.src = "data:image/svg+xml;utf8," + encodeURIComponent(svg);
};
