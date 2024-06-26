

public class ReasonChange : IReasonChange
{
	public ReasonChange(string heading, string details)
	{
		Heading = heading;
		Details = details;
	}

	public string Heading { get; set; }
	public string Details { get; set; }

}


public interface IReasonChange
{
	public string Heading { get; set; }
	public string Details { get; set; }
}
