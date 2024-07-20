﻿using LibrarySystem.Application.Interfaces.Services;

namespace LibrarySystem.Application.Interfaces;

public interface IServiceFactory
{
    IUserService CreateUserService();
    IEmployeeService CreateEmployeeService();
    IAuthorService CreateAuthorService();
    IGenreService CreateGenreService();
    IBookService CreateBookService();
    IBorrowService CreateBorrowService();
    IBookReviewService CreateBookReviewService();
    IWishlistService CreateWishlistService();
    IPictureService CreatePictureService();
}
