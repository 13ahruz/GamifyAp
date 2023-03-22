using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BbAuthContext
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public string scope { get; set; }
    public string user_id { get; set; }
}