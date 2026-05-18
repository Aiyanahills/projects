document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('contactForm');
    const formSuccess = document.getElementById('formSuccess');
    const formError = document.getElementById('formError');

    form.addEventListener('submit', async function(e) {
        e.preventDefault();

        const formData = {
            userName: document.getElementById('userName').value,
            topic: document.getElementById('topic').value,
            email: document.getElementById('email').value,
            text: document.getElementById('text').value
        };

        try {
            const response = await fetch('/api/contact', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            const result = await response.json();

            if (response.ok && result.success) {
                formSuccess.style.display = 'block';
                formError.style.display = 'none';
                form.reset();
                
                setTimeout(() => {
                    formSuccess.style.display = 'none';
                }, 3000);
            } else {
                throw new Error(result.message || 'Ошибка отправки');
            }
        } catch (error) {
            formError.style.display = 'block';
            formSuccess.style.display = 'none';
            console.error('Error:', error);
            
            setTimeout(() => {
                formError.style.display = 'none';
            }, 3000);
        }
    });
});