﻿using InventorySystem_Application.Common;
using MediatR;

namespace InventorySystem_Application.ProductCategory.Command.CreateCommand;

public record ProductCategoryCreateCommand(int CategoryId, string CategoryProductName, string Description, bool IsActive) : 
    IRequest<IResult<int>>;