
document.addEventListener("DOMContentLoaded", function () {
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    document.querySelectorAll(".rating").forEach(rating => {
        const stars = rating.querySelectorAll(".star");
        const productId = rating.dataset.productId;

        stars.forEach(star => {
            star.style.cursor = "pointer";

            // Подсветка при наведении (только изменение иконки на solid)
            star.addEventListener("mouseover", function () {
                const value = this.dataset.value;
                highlightStars(stars, value, true); // true - при наведении, без подсветки
            });

            // Возврат к текущему рейтингу при отведении мыши
            star.addEventListener("mouseout", function () {
                resetStars(rating);
            });

            // Отправка оценки
            star.addEventListener("click", async function () {
                const value = this.dataset.value;
                try {
                    const response = await fetch("/Home/Rate", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/x-www-form-urlencoded",
                            "RequestVerificationToken": token
                        },
                        body: `productId=${productId}&rating=${value}`
                    });

                    if (!response.ok) {
                        alert("Авторизуйтесь что-бы оставить оценку...");
                        return;
                    }

                    const result = await response.json();
                    if (result.success) {
                        // Обновляем data-rating на элементе
                        rating.dataset.rating = result.newRating;

                        // Пересчитываем и обновляем подсветку звезд
                        highlightStars(stars, result.newRating, false); // false - без изменений
                        // Обновляем количество отзывов
                        rating.querySelector(".rating-count").textContent = `(${result.ratingCount})`;
                    }
                } catch (error) {
                    console.error("Ошибка:", error);
                }
            });
        });
    });
});

// Функция для подсветки звезд
function highlightStars(stars, rating, hover) {
    stars.forEach((star, index) => {
        const icon = star.querySelector(".fa-star");

        // Если находимся в режиме наведения (hover) - меняем на fa-solid
        if (hover) {
            if (index < rating) {
                icon.classList.add("fa-solid");
                icon.classList.remove("fa-regular");
            } else {
                icon.classList.add("fa-regular");
                icon.classList.remove("fa-solid");
            }
        } else {
            // Если это не hover, оставляем как есть
            if (index < rating) {
                icon.classList.add("fa-solid");
                icon.classList.remove("fa-regular");
            } else {
                icon.classList.add("fa-regular");
                icon.classList.remove("fa-solid");
            }
        }
    });
}

// Функция для сброса состояния звезд при отведении мыши
function resetStars(rating) {
    const stars = rating.querySelectorAll(".star");
    const currentRating = rating.dataset.rating || 0;
    highlightStars(stars, currentRating, false); // false - возвращаем в текущий рейтинг
}
