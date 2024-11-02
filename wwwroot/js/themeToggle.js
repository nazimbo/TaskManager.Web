'use strict';

const ThemeToggle = () => {
    const [isDark, setIsDark] = React.useState(false);

    React.useEffect(() => {
        // Check initial theme
        const checkTheme = () => {
            const theme = document.documentElement.getAttribute('data-bs-theme');
            setIsDark(theme === 'dark');
        };

        // Initial check
        checkTheme();

        // Listen for theme changes
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                if (mutation.attributeName === 'data-bs-theme') {
                    checkTheme();
                }
            });
        });

        observer.observe(document.documentElement, {
            attributes: true,
            attributeFilter: ['data-bs-theme']
        });

        // Listen for custom theme change event
        window.addEventListener('themechange', checkTheme);

        return () => {
            observer.disconnect();
            window.removeEventListener('themechange', checkTheme);
        };
    }, []);

    const handleThemeToggle = (e) => {
        e.preventDefault();
        window.toggleTheme();
    };

    const buttonStyle = {
        fontSize: '1.25rem',
        color: isDark ? '#ffc107' : '#0d6efd',
        transition: 'color 0.3s ease'
    };

    return React.createElement(
        'button',
        {
            onClick: handleThemeToggle,
            className: 'theme-toggle',
            'aria-label': 'Toggle theme',
            title: isDark ? 'Switch to light mode' : 'Switch to dark mode'
        },
        isDark
            ? React.createElement('i', {
                className: 'bi bi-sun-fill',
                style: buttonStyle
              })
            : React.createElement('i', {
                className: 'bi bi-moon-fill',
                style: buttonStyle
              })
    );
};