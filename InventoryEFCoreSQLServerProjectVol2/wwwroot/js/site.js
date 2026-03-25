document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');

    const applyTheme = (theme) => {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('theme', theme);

        if (themeIcon) {
            themeIcon.className = theme === 'dark'
                ? 'bi bi-sun-fill'
                : 'bi bi-moon-stars';
        }
        
        console.log('Theme changed to:', theme);
    };

    const currentTheme = localStorage.getItem('theme') || 'light';
    applyTheme(currentTheme);

    if (themeToggle) {
        themeToggle.addEventListener('click', () => {
            const activeTheme = document.documentElement.getAttribute('data-bs-theme') || 'light';
            const newTheme = activeTheme === 'dark' ? 'light' : 'dark';
            applyTheme(newTheme);
        });
    }
});

// Simple working functions
window.loadProducts = async function(page = 1) {
    console.log('=== LOAD PRODUCTS START ===', page);
    
    try {
        // Правилни ID-та от HTML
        const search = document.getElementById('searchInput')?.value?.trim() || '';
        const minQuantity = document.getElementById('filterMinQuantity')?.value || '';
        const maxPrice = document.getElementById('filterMaxPrice')?.value || '';
        const supplier = document.getElementById('filterSupplier')?.value || '';
        
        let url = `/api/products?page=${page}&pageSize=20`;
        if (search) url += `&search=${encodeURIComponent(search)}`;
        if (minQuantity) url += `&minQuantity=${minQuantity}`;
        if (maxPrice) url += `&maxPrice=${maxPrice}`;
        if (supplier) url += `&supplier=${encodeURIComponent(supplier)}`;
        
        console.log('Fetching URL:', url);
        
        const response = await fetch(url);
        console.log('Response status:', response.status);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const data = await response.json();
        console.log('Data received:', data);
        
        const tbody = document.getElementById('productsTableBody');
        if (!tbody) {
            console.error('Table body not found!');
            return;
        }
        
        console.log('Table body found, clearing...');
        tbody.innerHTML = '';
        
        if (data.items && data.items.length > 0) {
            console.log(`Found ${data.items.length} products, rendering...`);
            data.items.forEach((product, index) => {
                console.log(`Rendering product ${index + 1}:`, product);
                
                const row = tbody.insertRow();
                const createdDate = new Date(product.createdOnUtc).toLocaleDateString('bg-BG');
                
                row.innerHTML = `
                    <td>${product.id}</td>
                    <td>${product.name}</td>
                    <td class="text-center">${product.quantity}</td>
                    <td class="text-end">${product.price.toFixed(2)} лв.</td>
                    <td class="text-end">${(product.quantity * product.price).toFixed(2)} лв.</td>
                    <td>${product.supplier}</td>
                    <td>${createdDate}</td>
                    <td class="text-center">
                        <button class="btn btn-sm btn-outline-primary me-1" onclick="editProduct(${product.id})">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-outline-danger" onclick="deleteProduct(${product.id})">
                            <i class="bi bi-trash"></i>
                        </button>
                    </td>
                `;
                
                console.log(`Row ${index + 1} added to table`);
            });
            
            console.log('All products rendered successfully!');
        } else {
            console.log('No products found, showing empty message');
            tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted py-4">Няма намерени продукти</td></tr>';
        }
        
    } catch (error) {
        console.error('Load products error:', error);
        const tbody = document.getElementById('productsTableBody');
        if (tbody) {
            tbody.innerHTML = `<tr><td colspan="8" class="text-center text-danger py-4">Грешка: ${error.message}</td></tr>`;
        }
    }
    
    console.log('=== LOAD PRODUCTS END ===');
};

window.loadDashboard = async function() {
    console.log('Loading dashboard...');
    
    try {
        // Използваме същия endpoint като loadProducts, но с по-голям pageSize
        const response = await fetch('/api/products?page=1&pageSize=50');
        if (!response.ok) {
            throw new Error('Неуспешно зареждане на таблото');
        }
        
        const data = await response.json();
        console.log('Dashboard data:', data);
        
        if (data.items && data.items.length > 0) {
            const totalProducts = data.totalCount || data.items.length;
            const totalUnits = data.items.reduce((sum, p) => sum + (p.quantity || 0), 0);
            const totalValue = data.items.reduce((sum, p) => sum + ((p.quantity || 0) * (p.price || 0)), 0);
            const lowStock = data.items.filter(p => (p.quantity || 0) < 10).length;
            
            // Правилни ID-та от HTML
            const totalProductsEl = document.getElementById('totalProducts');
            const totalUnitsEl = document.getElementById('totalUnits');
            const totalValueEl = document.getElementById('totalValue');
            const lowStockEl = document.getElementById('lowStock');
            
            if (totalProductsEl) totalProductsEl.textContent = totalProducts;
            if (totalUnitsEl) totalUnitsEl.textContent = totalUnits;
            if (totalValueEl) totalValueEl.textContent = totalValue.toFixed(2) + ' лв.';
            if (lowStockEl) lowStockEl.textContent = lowStock;
            
            console.log('Dashboard updated:', { totalProducts, totalUnits, totalValue, lowStock });
        } else {
            // Задаваме нули ако няма данни
            const totalProductsEl = document.getElementById('totalProducts');
            const totalUnitsEl = document.getElementById('totalUnits');
            const totalValueEl = document.getElementById('totalValue');
            const lowStockEl = document.getElementById('lowStock');
            
            if (totalProductsEl) totalProductsEl.textContent = '0';
            if (totalUnitsEl) totalUnitsEl.textContent = '0';
            if (totalValueEl) totalValueEl.textContent = '0.00 лв.';
            if (lowStockEl) lowStockEl.textContent = '0';
        }
        
    } catch (error) {
        console.error('Dashboard error:', error);
        // Задаваме нули при грешка
        const totalProductsEl = document.getElementById('totalProducts');
        const totalUnitsEl = document.getElementById('totalUnits');
        const totalValueEl = document.getElementById('totalValue');
        const lowStockEl = document.getElementById('lowStock');
        
        if (totalProductsEl) totalProductsEl.textContent = '0';
        if (totalUnitsEl) totalUnitsEl.textContent = '0';
        if (totalValueEl) totalValueEl.textContent = '0.00 лв.';
        if (lowStockEl) lowStockEl.textContent = '0';
    }
};

window.addProduct = async function() {
    console.log('=== ADD PRODUCT START ===');
    
    try {
        const name = document.getElementById('productName')?.value?.trim() || '';
        const quantity = parseInt(document.getElementById('productQuantity')?.value || '0');
        const price = parseFloat(document.getElementById('productPrice')?.value || '0');
        const supplier = document.getElementById('productSupplier')?.value?.trim() || '';
        
        console.log('Form values:', { name, quantity, price, supplier });
        
        // Validation
        if (!name) {
            alert('Моля, въведете име на продукта!');
            return;
        }
        
        if (!supplier) {
            alert('Моля, въведете доставчик!');
            return;
        }
        
        if (isNaN(quantity) || quantity < 0) {
            alert('Моля, въведете валидно количество!');
            return;
        }
        
        if (isNaN(price) || price < 0) {
            alert('Моля, въведете валидна цена!');
            return;
        }
        
        const product = { name, quantity, price, supplier };
        console.log('Product object:', product);
        
        const response = await fetch('/api/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(product)
        });
        
        console.log('Add response status:', response.status);
        
        if (response.ok) {
            console.log('Product added successfully');
            alert('Продуктът е добавен успешно!');
            
            // Close modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('addProductModal'));
            if (modal) modal.hide();
            
            // Reset form
            const form = document.getElementById('addProductForm');
            if (form) form.reset();
            
            // Refresh data
            await loadProducts();
            await loadDashboard();
        } else {
            const errorText = await response.text();
            console.error('Add error:', errorText);
            alert('Грешка при добавяне на продукт: ' + errorText);
        }
        
    } catch (error) {
        console.error('Add product error:', error);
        alert('Грешка: ' + error.message);
    }
    
    console.log('=== ADD PRODUCT END ===');
};

window.editProduct = async function(id) {
    console.log('Edit product:', id);
    // TODO: Implement edit functionality
    alert('Редактиране на продукт ' + id);
};

window.deleteProduct = async function(id) {
    console.log('Delete product:', id);
    
    if (!confirm('Сигурни ли сте, че искате да изтриете този продукт?')) {
        return;
    }
    
    try {
        const response = await fetch(`/api/products/${id}`, { method: 'DELETE' });
        
        if (response.ok) {
            alert('Продуктът е изтрит успешно!');
            loadProducts();
            loadDashboard();
        } else {
            alert('Грешка при изтриване на продукт');
        }
        
    } catch (error) {
        console.error('Delete error:', error);
        alert('Грешка: ' + error.message);
    }
};

// Initialize
document.addEventListener('DOMContentLoaded', function() {
    console.log('=== DOM LOADED ===');
    
    // Load data immediately
    console.log('Starting product load...');
    loadProducts();
    
    // Load dashboard after products
    setTimeout(() => {
        console.log('Starting dashboard load...');
        loadDashboard();
    }, 1000);
    
    // Add Enter key listener for search
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                loadProducts(1); // Reload with filters
            }
        });
    }
    
    // Manual test button (optional)
    window.testLoadProducts = function() {
        console.log('Manual test triggered');
        loadProducts();
    };
    
    console.log('=== INITIALIZATION COMPLETE ===');
});
