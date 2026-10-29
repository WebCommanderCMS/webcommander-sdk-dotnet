namespace WebCommander.Resources.Categories.CategoriesDto;

/// <summary>Another record a category points at, read as <c>{id, name}</c>; a write needs only <see cref="Id"/>.</summary>
public sealed class CategoryReference : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }
}

/// <summary>The <c>general</c> section as read; <see cref="FilterProfile"/> reads <c>null</c> without the filter plugin.</summary>
public sealed class CategoryGeneral : Dto
{
    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Sku { get; set; }

    public string? Url { get; set; }

    public string? Heading { get; set; }

    public CategoryReference? ParentCategory { get; set; }

    public object? FilterProfile { get; set; }

    public CategoryReference? Layout { get; set; }

    public string? Summary { get; set; }
}

/// <summary>The <c>description</c> section, the same one field read and written.</summary>
public sealed class CategoryDescription : Dto
{
    public string? Content { get; set; }
}

/// <summary>The <c>images_media</c> section as read: the two stored images' URLs and the image's alt text, each
/// <c>""</c> when unset.</summary>
public sealed class CategoryImagesMedia : Dto
{
    public string? ImageUrl { get; set; }

    public string? ImageAlt { get; set; }

    public string? BackgroundImageUrl { get; set; }
}

/// <summary>Who <c>available_for: "selected"</c> admits, each customer and group read as <c>{id, name}</c>.</summary>
public sealed class CategorySelectedCustomers : Dto
{
    public IReadOnlyList<CategoryReference?>? Customers { get; set; }

    public IReadOnlyList<CategoryReference?>? Groups { get; set; }
}

/// <summary>The <c>availability_visibility</c> section as read; <c>password</c> is never echoed.</summary>
public sealed class CategoryAvailabilityVisibility : Dto
{
    public bool? Available { get; set; }

    public bool? Visible { get; set; }

    public string? AvailableFor { get; set; }

    public CategorySelectedCustomers? SelectedCustomers { get; set; }

    public bool? AvailableOnDateRange { get; set; }

    public string? AvailableFromDate { get; set; }

    public string? AvailableToDate { get; set; }

    public bool? PasswordProtected { get; set; }

    public bool? CustomSorting { get; set; }

    public string? DefaultSorting { get; set; }

    public bool? DisableTracking { get; set; }
}

/// <summary>The <c>tax_shipping</c> section, the same eight fields read and written; naming a
/// <see cref="ShippingProfile"/> sets <see cref="EnableShipping"/>, and <c>false</c> there clears it.</summary>
public sealed class CategoryTaxShipping : Dto
{
    public bool? TaxExempt { get; set; }

    public CategoryReference? TaxProfile { get; set; }

    public string? TaxMessage { get; set; }

    public bool? ShowPriceWithTax { get; set; }

    public bool? PricesEnteredWithTax { get; set; }

    public string? BasePriceRounding { get; set; }

    public bool? EnableShipping { get; set; }

    public CategoryReference? ShippingProfile { get; set; }
}

/// <summary>One assigned product, as <c>assign_products</c> reads it.</summary>
public sealed class CategoryProduct : Dto
{
    public long? Id { get; set; }

    public string? Name { get; set; }

    public string? Sku { get; set; }
}

/// <summary>The <c>assign_products</c> section as read.</summary>
public sealed class CategoryAssignProducts : Dto
{
    public IReadOnlyList<CategoryProduct?>? Products { get; set; }
}

/// <summary>One <c>group_meta</c> entry; <see cref="Href"/> keeps the row's id as the store's own placeholder.</summary>
public sealed class SectionMeta : Dto
{
    public string? Kind { get; set; }

    public bool? Writable { get; set; }

    public string? Href { get; set; }
}

/// <summary>The <c>group_meta</c> block the listing, a row read and every whole-row write send.</summary>
public sealed class GroupMeta : Dto
{
    public SectionMeta? General { get; set; }

    public SectionMeta? Description { get; set; }

    public SectionMeta? ImagesMedia { get; set; }

    public SectionMeta? AvailabilityVisibility { get; set; }

    public SectionMeta? TaxShipping { get; set; }

    public SectionMeta? AssignProducts { get; set; }
}

/// <summary>One category: a listing row, a row read, a whole-row write's answer, or a section's
/// answer, which sets that one section alone.</summary>
public sealed class Category : Dto
{
    protected override string? EnvelopeKey => "category";

    public CategoryGeneral? General { get; set; }

    public CategoryDescription? Description { get; set; }

    public CategoryImagesMedia? ImagesMedia { get; set; }

    public CategoryAvailabilityVisibility? AvailabilityVisibility { get; set; }

    public CategoryTaxShipping? TaxShipping { get; set; }

    public CategoryAssignProducts? AssignProducts { get; set; }

    public long? Id { get; set; }

    /// <summary>The number the category was filed under, <c>null</c> when none.</summary>
    public string? TransactionNo { get; set; }

    public bool? Disposable { get; set; }

    public bool? InTrash { get; set; }

    /// <summary>Read <c>false</c> on a child whose parent is in the trash.</summary>
    public bool? ParentInTrash { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedOn { get; set; }

    public string? LastUpdatedBy { get; set; }

    public string? LastUpdatedOn { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    /// <summary><c>object</c>, the schema's entries differing in width.</summary>
    public object? FieldMetadata { get; set; }
}

/// <summary>The listing's ten-key <c>pagination</c> block; <see cref="Limit"/> is the size the store applied.</summary>
public sealed class Pagination : Dto
{
    public long? Total { get; set; }

    public long? Limit { get; set; }

    public long? Offset { get; set; }

    public long? Count { get; set; }

    public long? CurrentPage { get; set; }

    public long? TotalPages { get; set; }

    public bool? HasNext { get; set; }

    public bool? HasPrevious { get; set; }

    public string? PreviousPage { get; set; }

    public string? NextPage { get; set; }
}

/// <summary>One page of the listing, with the schema when it was asked for.</summary>
public sealed class CategoryPage : Dto
{
    public IReadOnlyList<Category?>? Categories { get; set; }

    public GroupMeta? GroupMeta { get; set; }

    public Pagination? Pagination { get; set; }

    public object? FieldMetadata { get; set; }
}

/// <summary>The count sub-path's <c>{"categories": {"count": N}}</c>.</summary>
public sealed class CategoryCount : Dto
{
    protected override string? EnvelopeKey => "categories";

    public long? Count { get; set; }
}

/// <summary>The writable <c>general</c> fields; a <c>PUT</c> that omits <see cref="Name"/> keeps it.</summary>
public sealed class CategoryGeneralInput : Dto
{
    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Sku { get; set; }

    public string? Url { get; set; }

    public string? Heading { get; set; }

    public CategoryReference? ParentCategory { get; set; }

    public CategoryReference? Layout { get; set; }

    public string? Summary { get; set; }
}

/// <summary>The writable <c>images_media</c> fields, each image <c>{filename, base64}</c> and read back as its
/// <c>*_url</c>.</summary>
public sealed class CategoryImagesMediaInput : Dto
{
    public IReadOnlyDictionary<string, object?>? Image { get; set; }

    public string? ImageAlt { get; set; }

    public IReadOnlyDictionary<string, object?>? BackgroundImage { get; set; }
}

/// <summary>The ids <c>available_for: "selected"</c> admits; an id nothing holds is dropped without a refusal.</summary>
public sealed class CategorySelectedCustomersInput : Dto
{
    public IReadOnlyList<long?>? Customers { get; set; }

    public IReadOnlyList<long?>? Groups { get; set; }
}

/// <summary>The writable <c>availability_visibility</c> fields: the read fields plus the write-only <see cref="Password"/>.</summary>
public sealed class CategoryAvailabilityVisibilityInput : Dto
{
    public bool? Available { get; set; }

    public bool? Visible { get; set; }

    public string? AvailableFor { get; set; }

    public CategorySelectedCustomersInput? SelectedCustomers { get; set; }

    public bool? AvailableOnDateRange { get; set; }

    public string? AvailableFromDate { get; set; }

    public string? AvailableToDate { get; set; }

    public bool? PasswordProtected { get; set; }

    public string? Password { get; set; }

    public bool? CustomSorting { get; set; }

    public string? DefaultSorting { get; set; }

    public bool? DisableTracking { get; set; }
}

/// <summary>The product ids to assign; a write replaces the whole assignment.</summary>
public sealed class CategoryAssignProductsInput : Dto
{
    public IReadOnlyList<long?>? Products { get; set; }
}

/// <summary>The body every write takes, <c>{"category": {&lt;section&gt;: {...}}}</c>: a create, a
/// <c>PUT</c>, a <c>PATCH</c> and a section <c>PATCH</c>, the last reading only its own section.</summary>
public sealed class CategoryInput : Dto
{
    protected override string? EnvelopeKey => "category";

    public CategoryGeneralInput? General { get; set; }

    public CategoryDescription? Description { get; set; }

    public CategoryImagesMediaInput? ImagesMedia { get; set; }

    public CategoryAvailabilityVisibilityInput? AvailabilityVisibility { get; set; }

    public CategoryTaxShipping? TaxShipping { get; set; }

    public CategoryAssignProductsInput? AssignProducts { get; set; }

    /// <summary>Read inside the <c>category</c> wrapper only; beside it, it is ignored and nothing is filed.</summary>
    public string? TransactionNo { get; set; }
}
