// Simple working JavaScript
console.log('Script loaded');

// Global functions
window.loadProducts = async function(page = 1) {
    console.log('Loading products...');
    
    try {
        const response = await fetch(`/api/products?page=${page}&pageSize=20`);
        const data = await response.json();
        console.log('Data:', data);
        
        const tbody = document.getElementById('productsTableBody');
        if (!tbody) {
            console.error('Table body not found');
            return;
        }
        
        tbody.innerHTML = '';
        
        if (data.items && data.items.length > 0) {
            data.items.forEach(product => {
                const row = tbody.insertRow();
                row.innerHTML = `
                    <td>${product.id}</td>
                    <td>${product.name}</td>
                    <td class="text-center">${product.quantity}</td>
                    <td class="text-end">${product.price.toFixed(2)} лв.</td>
                    <td class="text-end">${(product.quantity * product.price).toFixed(2)} лв.</td>
                    <td>${product.supplier}</td>
                    <td>${new Date(product.createdOnUtc).toLocaleDateString('bg-BG')}</td>
                    <td class="text-center">
                        <button class="btn btn-sm btn-outline-primary me-1" onclick="editProduct(${product.id})">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-outline-danger" onclick="deleteProduct(${product.id})">
                            <i class="bi bi-trash"></i>
                        </button>
                    </td>
                `;
            });
        } else {
            tbody.innerHTML = '<tr><td colspan="8" class="text-center text-muted">Няма продукти</td></tr>';
        }
        
        // Update total records
        const totalRecords = document.getElementById('totalRecords');
        if (totalRecords) {
            totalRecords.textContent = `Общо: ${data.totalCount || 0} записа`;
        }
        
    } catch (error) {
        console.error('Error:', error);
        const tbody = document.getElementById('productsTableBody');
        if (tbody) {
            tbody.innerHTML = '<tr><td colspan="8" class="text-center text-danger">Грешка: ' + error.message + '</td></tr>';
        }
    }
};

window.addProduct = async function() {
    console.log('Adding product...');
    
    try {
        const name = document.getElementById('productName')?.value?.trim() || '';
        const quantity = parseInt(document.getElementById('productQuantity')?.value || '0');
        const price = parseFloat(document.getElementById('productPrice')?.value || '0');
        const supplier = document.getElementById('productSupplier')?.value?.trim() || '';
        
        if (!name || !supplier || isNaN(quantity) || isNaN(price)) {
            alert('Моля, попълни всички полета коректно!');
            return;
        }
        
        const product = { name, quantity, price, supplier };
        
        const response = await fetch('/api/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(product)
        });
        
        if (response.ok) {
            alert('Продуктът е добавен успешно!');
            const modal = bootstrap.Modal.getInstance(document.getElementById('addProductModal'));
            if (modal) modal.hide();
            document.getElementById('addProductForm').reset();
            await loadProducts();
        } else {
            alert('Грешка при добавяне на продукт');
        }
        
    } catch (error) {
        console.error('Error:', error);
        alert('Грешка: ' + error.message);
    }
};

window.editProduct = function(id) {
    console.log('Edit product:', id);
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
            await loadProducts();
        } else {
            alert('Грешка при изтриване на продукт');
        }
        
    } catch (error) {
        console.error('Error:', error);
        alert('Грешка: ' + error.message);
    }
};

// Load dashboard
window.loadDashboard = async function() {
    console.log('Loading dashboard...');
    
    try {
        const response = await fetch('/api/products?page=1&pageSize=50');
        const data = await response.json();
        
        if (data.items && data.items.length > 0) {
            const totalProducts = data.totalCount || data.items.length;
            const totalUnits = data.items.reduce((sum, p) => sum + (p.quantity || 0), 0);
            const totalValue = data.items.reduce((sum, p) => sum + ((p.quantity || 0) * (p.price || 0)), 0);
            const lowStock = data.items.filter(p => (p.quantity || 0) < 10).length;
            
            const totalProductsEl = document.getElementById('totalProducts');
            const totalUnitsEl = document.getElementById('totalUnits');
            const totalValueEl = document.getElementById('totalValue');
            const lowStockEl = document.getElementById('lowStock');
            
            if (totalProductsEl) totalProductsEl.textContent = totalProducts;
            if (totalUnitsEl) totalUnitsEl.textContent = totalUnits;
            if (totalValueEl) totalValueEl.textContent = totalValue.toFixed(2) + ' лв.';
            if (lowStockEl) lowStockEl.textContent = lowStock;
        }
        
    } catch (error) {
        console.error('Dashboard error:', error);
    }
};

// Initialize on page load
document.addEventListener('DOMContentLoaded', function() {
    console.log('Page loaded');
    
    // Load data
    loadProducts();
    setTimeout(loadDashboard, 1000);
    
    // Add Enter key listener for search
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                e.preventDefault();
                loadProducts(1);
            }
        });
    }
});
