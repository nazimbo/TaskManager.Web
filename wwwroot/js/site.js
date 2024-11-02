// Handle task status toggle with animation
function handleTaskStatusToggle() {
    $('.task-status').change(function() {
        const checkbox = $(this);
        const taskId = checkbox.data('task-id');
        const row = checkbox.closest('tr');
        
        row.css('opacity', '0.5');
        
        $.post(`/TodoTask/ToggleStatus/${taskId}`)
            .done(function(response) {
                if (response.success) {
                    row.css('opacity', '1');
                } else {
                    checkbox.prop('checked', !checkbox.prop('checked'));
                    row.css('opacity', '1');
                    showAlert('Failed to update task status', 'danger');
                }
            })
            .fail(function() {
                checkbox.prop('checked', !checkbox.prop('checked'));
                row.css('opacity', '1');
                showAlert('Failed to update task status', 'danger');
            });
    });
}

// Show alert message
function showAlert(message, type = 'success') {
    const alertHtml = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    const alertContainer = $('#alert-container');
    if (alertContainer.length) {
        alertContainer.html(alertHtml);
    } else {
        $('main').prepend(`<div id="alert-container">${alertHtml}</div>`);
    }
}

// Update due date formatting
function updateDueDateDisplay() {
    $('.due-date').each(function() {
        const dueDate = new Date($(this).data('date'));
        const today = new Date();
        const diffDays = Math.ceil((dueDate - today) / (1000 * 60 * 60 * 24));
        
        if (diffDays < 0) {
            $(this).addClass('overdue');
        } else if (diffDays <= 3) {
            $(this).addClass('upcoming');
        }
    });
}

// Initialize all tooltips
function initTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Document ready handler
$(document).ready(function() {
    handleTaskStatusToggle();
    updateDueDateDisplay();
    initTooltips();
    
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').alert('close');
    }, 5000);
});

// Initialize form validation
if ($.validator) {
    $.validator.setDefaults({
        highlight: function(element) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function(element) {
            $(element).removeClass('is-invalid');
        }
    });
}