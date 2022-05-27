const editReviewBtn = document.querySelectorAll(".editReviewBtn")
Array.from(editReviewBtn).forEach(editReview => {
    editReview.addEventListener("click", (edit) => {
        const reviewId = edit.target.dataset.reviewid
        const hiddenInputReviewId = document.getElementById("hiddenEditReviewId")
        hiddenInputReviewId.setAttribute("value", `${reviewId}`)

        const reviewContent = edit.target.parentElement.parentElement.parentElement.children[0].innerText

        $('#editReviewModal').modal('toggle')
        const textArea = document.getElementById("editReviewModalTextArea")
        textArea.value = reviewContent
    })
});
//


//Delete Review
const deleteReviewBtn = document.querySelectorAll(".deleteReviewBtn")
Array.from(deleteReviewBtn).forEach(deleteReview => {
    deleteReview.addEventListener("click", (d) => {
        const reviewId = d.target.dataset.reviewid;
        const hiddenInputReviewId = document.getElementById("hiddenReviewId");
        hiddenInputReviewId.setAttribute("value", `${reviewId}`);
        $('#deleteReviewModal').modal('toggle');
    })
})