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
}

/* ==========================================================================
   2. Bộ lọc Danh mục, Nhu cầu, Khoảng giá & Tìm kiếm nhanh
   ========================================================================== */
function initProductCatalogFilter() {
    const productCols = document.querySelectorAll(".dt-product-col");
    if (!productCols.length) return;

    const categoryButtons = document.querySelectorAll("[data-filter-cat]");
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

    // Nút liên kết nhanh từ Navbar dropdown
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
            catalogSection.scrollIntoView({ behavior: "smooth", block: "start" });
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
   3. Công cụ "Build PC cùng DTGear" (Kiểm tra tương thích & Tính tổng tiền)
   ========================================================================== */
function initBuildPcCalculator() {
    const builderRoot = document.getElementById("dtPcBuilderApp");
    if (!builderRoot) return;

    const partSelects = builderRoot.querySelectorAll(".js-pc-part-select");
    const accessoryCheckboxes = builderRoot.querySelectorAll(".js-pc-acc-checkbox");
    const presetButtons = builderRoot.querySelectorAll(".js-pc-preset-btn");

    const coreSubtotalEl = document.getElementById("dtCorePcSubtotal");
    const accSubtotalEl = document.getElementById("dtAccPcSubtotal");
    const grandTotalEl = document.getElementById("dtGrandPcTotal");
    const compatStatusBox = document.getElementById("dtCompatStatusBox");
    const compatTextEl = document.getElementById("dtCompatStatusText");
    const wattageTextEl = document.getElementById("dtWattageText");
    const selectedListEl = document.getElementById("dtSelectedPartsPreview");
    const applyToOrderBtn = document.getElementById("dtApplyBuildToOrderBtn");

    function formatVnd(amount) {
        return amount.toLocaleString("vi-VN") + " ₫";
    }

    function recalculateBuild() {
        let coreSubtotal = 0;
        let accSubtotal = 0;
        let estimatedWattage = 65; // Công suất nền hệ thống (quạt, tản nhiệt, ngoại vi)
        let psuCapacity = 0;

        let cpuSocket = "";
        let mainSocket = "";
        let mainRamType = "";
        let ramType = "";

        const selectedSummaryLines = [];

        partSelects.forEach((select) => {
            const opt = select.options[select.selectedIndex];
            if (!opt || !opt.value) return;

            const partType = select.dataset.partType || "";
            const name = opt.dataset.name || opt.textContent.trim();
            const price = parseFloat(opt.dataset.price || "0");
            const watt = parseInt(opt.dataset.wattage || "0", 10);
            const socket = opt.dataset.socket || "";
            const rType = opt.dataset.ramtype || "";

            coreSubtotal += price;
            selectedSummaryLines.push(`• [${partType}] ${name} (${formatVnd(price)})`);

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

            // Cập nhật hiển thị giá từng dòng bên phải select
            const rowPriceEl = document.getElementById(`price-${select.id}`);
            if (rowPriceEl) {
                rowPriceEl.textContent = formatVnd(price);
            }
        });

        const selectedAccessories = [];
        accessoryCheckboxes.forEach((cb) => {
            const card = cb.closest(".dt-accessory-check-card");
            if (cb.checked) {
                const name = cb.dataset.name || "";
                const price = parseFloat(cb.dataset.price || "0");
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

        if (wattageTextEl) {
            wattageTextEl.textContent = `Tiêu thụ ước tính ~${estimatedWattage}W / Nguồn đã chọn: ${psuCapacity}W`;
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
            if (issues.length === 0) {
                compatStatusBox.className = "dt-compat-badge dt-compat-ok mb-3";
                compatTextEl.textContent = "Tương thích 100% — Các linh kiện đồng bộ chuẩn mực";
            } else {
                compatStatusBox.className = "dt-compat-badge dt-compat-warn mb-3";
                compatTextEl.textContent = "Lưu ý tương thích: " + issues.join(" ");
            }
        }

        if (selectedListEl) {
            const previewHtml = selectedSummaryLines
                .slice(0, 5)
                .map((line) => `<div class="small text-secondary mb-1 text-truncate">${line}</div>`)
                .join("");
            const extraCount = selectedSummaryLines.length - 5 + selectedAccessories.length;
            selectedListEl.innerHTML =
                previewHtml +
                (extraCount > 0
                    ? `<div class="small fw-semibold text-primary mt-1">+ ${extraCount} linh kiện & phụ kiện khác trong cấu hình</div>`
                    : "");
        }

        // Lưu tạm vào dataset để chuyển nhanh xuống Form tư vấn/đặt hàng
        builderRoot.dataset.FullSummary = [
            "=== CẤU HÌNH BUILD PC CÙNG DTGEAR ===",
            ...selectedSummaryLines,
            ...(selectedAccessories.length ? ["--- Phụ kiện & Gear đi kèm ---", ...selectedAccessories] : []),
            `=> TỔNG CHI PHÍ TẠM TÍNH: ${formatVnd(grandTotal)}`
        ].join("\n");
        builderRoot.dataset.TotalAmount = grandTotal.toString();
    }

    partSelects.forEach((select) => {
        select.addEventListener("change", () => {
            presetButtons.forEach((b) => b.classList.remove("active"));
            recalculateBuild();
        });
    });

    accessoryCheckboxes.forEach((cb) => {
        cb.addEventListener("change", recalculateBuild);
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

            recalculateBuild();
        });
    });

    // Nút chuyển cấu hình Build PC xuống Form đặt hàng / nhận báo giá
    if (applyToOrderBtn) {
        applyToOrderBtn.addEventListener("click", () => {
            recalculateBuild();
            const summary = builderRoot.dataset.FullSummary || "";
            const total = parseFloat(builderRoot.dataset.TotalAmount || "0");

            const formSummaryEl = document.getElementById("OrderForm_ConfigurationSummary");
            const formTotalEl = document.getElementById("OrderForm_TotalEstimatedAmount");
            const formTotalPreviewEl = document.getElementById("dtFormTotalDisplay");

            if (formSummaryEl) {
                formSummaryEl.value = summary;
            }
            if (formTotalEl) {
                formTotalEl.value = total;
            }
            if (formTotalPreviewEl) {
                formTotalPreviewEl.textContent = formatVnd(total);
            }

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

    recalculateBuild();
}

/* ==========================================================================
   4. Hệ thống Giỏ hàng & Mua ngay / Chọn vào cấu hình
   ========================================================================== */
function initCartSystem() {
    const STORAGE_KEY = "dtgear_cart_items_v1";
    const cartCountBadges = document.querySelectorAll(".js-dt-cart-count");
    const cartItemsContainer = document.getElementById("dtCartItemsContainer");
    const cartTotalEl = document.getElementById("dtCartModalTotal");
    const checkoutCartBtn = document.getElementById("dtCheckoutCartToFormBtn");
    const clearCartBtn = document.getElementById("dtClearCartBtn");

    function loadCart() {
        try {
            const raw = localStorage.getItem(STORAGE_KEY);
            return raw ? JSON.parse(raw) : [];
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
        return amount.toLocaleString("vi-VN") + " ₫";
    }

    function renderCartUI(items) {
        const totalQty = items.reduce((sum, item) => sum + item.qty, 0);
        const totalAmount = items.reduce((sum, item) => sum + item.price * item.qty, 0);

        cartCountBadges.forEach((badge) => {
            badge.textContent = totalQty.toString();
        });

        if (!cartItemsContainer) return;

        if (items.length === 0) {
            cartItemsContainer.innerHTML = `
                <div class="text-center py-5">
                    <p class="text-secondary mb-1">Giỏ hàng của bạn đang trống.</p>
                    <p class="small text-secondary mb-0">Hãy chọn Laptop, Linh kiện PC hoặc Phụ kiện (Tai nghe, USB, Chuột...) để nhận báo giá.</p>
                </div>`;
            if (cartTotalEl) cartTotalEl.textContent = "0 ₫";
            return;
        }

        cartItemsContainer.innerHTML = items
            .map(
                (item, idx) => `
            <div class="d-flex align-items-center justify-content-between py-3 border-bottom">
                <div class="pe-3">
                    <div class="small text-primary fw-semibold">${item.subCategory}</div>
                    <div class="fw-semibold text-dark">${item.name}</div>
                    <div class="small text-secondary">${formatVnd(item.price)} x ${item.qty}</div>
                </div>
                <div class="d-flex align-items-center gap-2">
                    <span class="fw-bold text-nowrap">${formatVnd(item.price * item.qty)}</span>
                    <button type="button" class="btn btn-sm btn-outline-secondary rounded-pill px-2 py-1 js-remove-cart-item" data-index="${idx}">
                        Xóa
                    </button>
                </div>
            </div>`
            )
            .join("");

        if (cartTotalEl) {
            cartTotalEl.textContent = formatVnd(totalAmount);
        }

        cartItemsContainer.querySelectorAll(".js-remove-cart-item").forEach((btn) => {
            btn.addEventListener("click", () => {
                const idx = parseInt(btn.dataset.index || "0", 10);
                const current = loadCart();
                current.splice(idx, 1);
                saveCart(current);
            });
        });
    }

    // Nút "Mua Ngay" trên thẻ sản phẩm -> Điền thẳng xuống Form đặt hàng & cuộn xuống
    document.querySelectorAll(".js-buy-now-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            const name = btn.dataset.name || "";
            const subcat = btn.dataset.subcat || "";
            const price = parseFloat(btn.dataset.price || "0");

            const formSummaryEl = document.getElementById("OrderForm_ConfigurationSummary");
            const formTotalEl = document.getElementById("OrderForm_TotalEstimatedAmount");
            const formTotalPreviewEl = document.getElementById("dtFormTotalDisplay");

            const line = `[Đặt mua sản phẩm DTGear] (${subcat}) ${name} — Giá: ${formatVnd(price)}`;
            if (formSummaryEl) {
                formSummaryEl.value = formSummaryEl.value
                    ? formSummaryEl.value + "\n" + line
                    : line;
            }
            if (formTotalEl) {
                const existing = parseFloat(formTotalEl.value || "0");
                const updated = existing + price;
                formTotalEl.value = updated;
                if (formTotalPreviewEl) {
                    formTotalPreviewEl.textContent = formatVnd(updated);
                }
            }

            const orderSection = document.getElementById("dt-order-section");
            if (orderSection) {
                orderSection.scrollIntoView({ behavior: "smooth", block: "start" });
            } else {
                window.location.href = "/#dt-order-section";
            }
        });
    });

    // Nút "Thêm vào cấu hình / Giỏ hàng" trên thẻ sản phẩm
    document.querySelectorAll(".js-add-cart-btn").forEach((btn) => {
        btn.addEventListener("click", () => {
            const id = btn.dataset.id || "";
            const name = btn.dataset.name || "";
            const subCategory = btn.dataset.subcat || "";
            const price = parseFloat(btn.dataset.price || "0");

            const items = loadCart();
            const existing = items.find((x) => x.id === id);
            if (existing) {
                existing.qty += 1;
            } else {
                items.push({ id, name, subCategory, price, qty: 1 });
            }
            saveCart(items);

            // Hiệu ứng phản hồi nhẹ nhàng trên nút bấm
            const originalHtml = btn.innerHTML;
            btn.innerHTML = `<span class="dt-icon-inline">✓ Đã thêm</span>`;
            btn.classList.add("active");
            setTimeout(() => {
                btn.innerHTML = originalHtml;
                btn.classList.remove("active");
                if (window.lucide) window.lucide.createIcons();
            }, 1100);
        });
    });

    // Chuyển toàn bộ Giỏ hàng xuống Form đặt hàng / nhận báo giá
    if (checkoutCartBtn) {
        checkoutCartBtn.addEventListener("click", () => {
            const items = loadCart();
            if (!items.length) return;

            const total = items.reduce((s, i) => s + i.price * i.qty, 0);
            const lines = [
                "=== DANH SÁCH SẢN PHẨM TRONG GIỎ HÀNG DTGEAR ===",
                ...items.map((i) => `• [${i.subCategory}] ${i.name} x${i.qty} = ${formatVnd(i.price * i.qty)}`),
                `=> TỔNG CỘNG: ${formatVnd(total)}`
            ].join("\n");

            const formSummaryEl = document.getElementById("OrderForm_ConfigurationSummary");
            const formTotalEl = document.getElementById("OrderForm_TotalEstimatedAmount");
            const formTotalPreviewEl = document.getElementById("dtFormTotalDisplay");

            if (formSummaryEl) formSummaryEl.value = lines;
            if (formTotalEl) formTotalEl.value = total;
            if (formTotalPreviewEl) formTotalPreviewEl.textContent = formatVnd(total);

            const orderSection = document.getElementById("dt-order-section");
            if (orderSection) {
                orderSection.scrollIntoView({ behavior: "smooth", block: "start" });
            } else {
                window.location.href = "/#dt-order-section";
            }
        });
    }

    if (clearCartBtn) {
        clearCartBtn.addEventListener("click", () => {
            saveCart([]);
        });
    }

    renderCartUI(loadCart());
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
