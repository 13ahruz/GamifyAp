using System.Collections.Generic;
using System;

[System.Serializable]
public class Course
{
    public string id;
    public string courseId;
    public string name;
    public bool organization;
    public string ultraStatus;
    public bool readOnly;
    public AvailabilityCourse availability;
    public EnrollmentCourse enrollment;
    public LocaleCourse locale;
    public string externalAccessUrl;
}

[System.Serializable]
public class MyCourses
{
    public List<Course> results { get; set; }
}

[System.Serializable]
public class Contents
{
    public List<Content> results;
}

[System.Serializable]
public class Content
{
    public string id;
    public string title;
    public DateTime created;
    public DateTime modified;
    public int position;
    public bool hasChildren;
    public bool launchInNewWindow;
    public bool reviewable;
    public Availability availability;
    public ContentHandlerResult contentHandler;
    public List<LinkResult> links;
}



[System.Serializable]
public class AvailabilityCourse
{
    public string available;
    public DurationCourse duration;
    public bool AllowGuests;
    public bool AllowObservers;
    public object AdaptiveRelease;
}

[System.Serializable]
public class DurationCourse
{
    public string type;
    public string start;
    public string end;
}

[System.Serializable]
public class LocaleCourse
{
    public bool force;
}

[System.Serializable]
public class EnrollmentCourse
{
    public string type;
}

[System.Serializable]
public class Announcement
{
    public string id;
    public string title;
    public string body;
    public string creator;
    public bool draft;
    public Availability available;
    public string created;
    public string modified;
    public int position;
}

[System.Serializable]
public class AnnouncementList
{
    public List<Announcement> results;
}


[System.Serializable]
public class CourseContentList
{
    public List<CourseContent> results;
}


[System.Serializable]
public class CourseContent
{
    public string id;
    public string parentId;
    public string title;
    public DateTime Created;
    public DateTime Modified;
    public int position;
    public bool launchInNewWindow;
    public bool reviewable;
    public Availability availability;
    public ContentHandlerCourseContent contentHandler;
    public List<LinkCourseContent> Links;
    internal object url;

}

[System.Serializable]
public class ContentHandlerCourseContent
{
    public string id;
    public FileCourseContent file;
    public string FileAssociationMode;
}

[System.Serializable]
public class FileCourseContent
{
    public string FileName;
}


[System.Serializable]
public class LinkCourseContent
{
    public string Href;
    public string Rel;
    public string Title;
    public string Type;
}


[System.Serializable]
public class Group
{
    public string id;
    public string externalId;
    public string parentId;
    public string name;
    public bool isGroupSet;
    public AvailabilityGroup availability;
    public EnrollmentGroup enrollment;
    public string uuid;
}

[System.Serializable]
public class AvailabilityGroup
{
    public string available;
}

[System.Serializable]
public class EnrollmentGroup
{
    public string type;
    public int limit;
    public SignupSheetGroup signupSheet;
}

[System.Serializable]
public class SignupSheetGroup
{
    public string name;
    public string description;
    public bool showMembers;
}

[System.Serializable]
public class GroupList
{
    public List<Group> results;
}

[System.Serializable]
public class Meeting
{
    public int id;
    public string courseId;
    public string start;
    public string end;
}


[System.Serializable]
public class MeetingList
{
    public List<Meeting> results;
}


[System.Serializable]
public class Attendance
{
    public int id;
    public string meetingId;
    public string userId;
    public string status;
}


[System.Serializable]
public class AttendanceList
{
    public List<Attendance> results;
}

[System.Serializable]
public class CalendarEvent
{
    public string id;
    public string type;
    public string calendarId;
    public string calendarName;
    public string title;
    public string description;
    public string location;
    public DateTime start;
    public DateTime end;
    public DateTime modified;
    public string color;
    public bool disableResizing;
    public string createdByUserId;
}

[System.Serializable]
public class CalendarList
{
    public List<CalendarEvent> results;
}



