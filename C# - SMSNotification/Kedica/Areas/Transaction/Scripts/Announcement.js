"use strict";
(function () {
    const Announcement = function () {
        return new Announcement.init();
    }
    Announcement.init = function () {
        $D.init.call(this);
        this.$tblAnnouncementList = "";
    }
    Announcement.prototype = {
        getAnnouncementList: function () {
            var self = this;
            self.formAction = '/Transaction/Announcement/GetAnnouncementListDetails';
            self.sendData().then(function () {
                self.populateAnnouncement(self.responseData.announcementList)
            });
        },
        populateAnnouncement: function (listdata) {
            var self = this;
            var vhtml = "";
            for (var i = 0; i < listdata.length; i++) {
                vhtml += '<div class="timeline-item">'
                vhtml += '   <div class="timeline-time">'
                vhtml += '        <span class="date">' + listdata[i].CreateDate.split(" ")[0] + '</span>'
                vhtml += '        <span class="time"> ' + listdata[i].CreateDate.split(" ")[1] + " " + listdata[i].CreateDate.split(" ")[2] +  ' </span>'
                vhtml += '   </div>'
                vhtml += '   <div class="timeline-icon">'
                vhtml += '       <a href="javascript:;">&nbsp;</a>'
                vhtml += '  </div>'
                vhtml += '    <div class="timeline-content">'
                vhtml += '       <div class="timeline-header">'
                vhtml += '           <div class="userimage"><img src="/Content/assets/img/userpic.png" alt=""></div>'
                vhtml += '               <div class="username">'
                vhtml += '                   <a href="javascript:;">   ' + listdata[i].PostedBy + ' <i class="fa fa-check-circle text-blue ms-1"></i></a>'
                vhtml += '               </div>'
                vhtml += '           </div>'
                vhtml += '           <div class="timeline-body">'
                vhtml += '               <div class="mb-3">'
                vhtml += '                   <h3 style="padding-top:10px !important"> ' + listdata[i].PostBody + '</h3>       '                        
                vhtml += '                  <div class="mb-2">'
                vhtml += '                       ' + listdata[i].Announcement + ''
                vhtml += '                  </div>'
                vhtml += '                  <div class="row gx-1">'
                vhtml += '                          <div class="col-12">'
                vhtml += '                              <img src="../Areas/Transaction/Uploads/' + listdata[i].attachmentName + '" alt="" class="mw-100 d-block">'
                vhtml += '                          </div'
                vhtml += '                  </div>'
                vhtml += '               </div>'
                vhtml += '           </div>'
                vhtml += '        </div>'
                vhtml += '   </div>'
            }
            $("#PopulateTimeLine").html(vhtml)


        },
       
    }
    Announcement.init.prototype = $.extend(Announcement.prototype, $D.init.prototype);
    Announcement.init.prototype = Announcement.prototype;
    var AnnouncementList = Announcement();
    var CUI2 = $UI();

    $(document).ready(function () {
        AnnouncementList.getAnnouncementList();


       
    });
})()