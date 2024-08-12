namespace NextechDemo.Shared;

/// <summary>
///     Response class for page data
/// </summary>
/// <typeparam name="T"></typeparam>
public class Response<T>
{
    /// <summary>
    ///     Page information
    /// </summary>
    public PageInfo PageInfo { get; set; }

    /// <summary>
    ///     Records returned from DB
    /// </summary>
    public T Results { get; set; }

    /// <summary>
    ///     Returns true, if there are records else false
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    ///     Message to be sent in response
    /// </summary>
    public string Message { get; set; }
}

/// <summary>
///     Contain page related infomation
/// </summary>
public class PageInfo
{
    /// <summary>
    ///     Current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    ///     No of records per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    ///     Total no of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    ///     Total no of records
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    ///     Flag to check if full data set required
    /// </summary>
    public bool IsFullDataRequired { get; set; }
}