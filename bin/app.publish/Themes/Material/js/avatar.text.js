/**
       * Generate an avatar image from letters
       */
      (function (w, d) {

          function AvatarImage(letters, size) {
              var canvas = d.createElement('canvas');
              var context = canvas.getContext("2d");
              var size = size || 60;

              // Generate a random color every time function is called
          	  //var color = "#" + (Math.random() * 0xFFFFFF << 0).toString(16);
              var color = "#000";

              // Set canvas with & height
              canvas.width = size;
              canvas.height = size;

              // Select a font family to support different language characters
              // like Arial
              context.font = Math.round(canvas.width / 2) + "px Arial";
              context.textAlign = "center";

              // Setup background and front color
              context.fillStyle = color;
              context.fillRect(0, 0, canvas.width, canvas.height);
              context.fillStyle = "#FFF";
              context.fillText(letters, size / 2, size / 1.5);

              // Set image representation in default format (png)
              dataURI = canvas.toDataURL();

              // Dispose canvas element
              canvas = null;

              return dataURI;
          }

          w.AvatarImage = AvatarImage;

      })(window, document);

      (function (w, d) {

      	function generateAvatars() {
      		let letter = $("#fullname").text();
      		let regexp = /(\b)([a-zA-Z])/g;
      		let matches = (letter.match(regexp) || []).map(e => e.replace(regexp, '$2'));      		
      		let text = matches.length == 1 ? matches[0].toString() : ((matches.length > 1 ) ? matches[0].toString() + "" + matches[1].toString() : "");
      		//$("img[src*=unknown]").attr("letters", text); //MAX 2 Letter (matches.join(""))
            $("img[src*=unknown]").each(function () {
                $(this).attr("letters", $(this).attr("data-initial"))
            });

      		var images = d.querySelectorAll('img[letters]');

      		for (var i = 0, len = images.length; i < len; i++) {
      			var img = images[i];
      			img.src = AvatarImage(img.getAttribute('letters'), img.getAttribute('width'));
      			img.removeAttribute('letters');
      		}
      	}

      	d.addEventListener('DOMContentLoaded', function (event) {
      		generateAvatars();
      	});

      })(window, document);