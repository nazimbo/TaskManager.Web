// Theme initialization
(function() {
    function setTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('theme', theme);
        
        // Dispatch custom event for theme change
        window.dispatchEvent(new CustomEvent('themechange', { 
            detail: { theme: theme } 
        }));
    }

    // Check for saved theme preference, otherwise use system preference
    const savedTheme = localStorage.getItem('theme');
    const systemPrefersDark = window.matchMedia('(prefers-color-scheme: dark)');
    
    if (savedTheme) {
        setTheme(savedTheme);
    } else {
        setTheme(systemPrefersDark.matches ? 'dark' : 'light');
    }
    
    // Listen for system theme changes
    systemPrefersDark.addEventListener('change', e => {
        if (!localStorage.getItem('theme')) {
            setTheme(e.matches ? 'dark' : 'light');
        }
    });

    // Make theme toggle function globally available
    window.toggleTheme = function() {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        setTheme(currentTheme === 'dark' ? 'light' : 'dark');
    };
})();