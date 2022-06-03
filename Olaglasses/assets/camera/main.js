const tryButton = document.getElementById("try-video-button");
const stopButton = document.getElementById("stop-video-button");
const container = document.getElementById("video-div");

async function startVideo() {
  startVideoUI();
  const track = await Twilio.Video.createLocalVideoTrack();
  container.append(track.attach());
  stopButton.addEventListener("click", function () {
    stopVideo(track);
  });
}
function stopVideo(track) {
  track.stop();
  endVideoUI();
}
function startVideoUI() {
  tryButton.disabled = true;
  stopButton.disabled = false;
  container.innerHTML = "";
}
function endVideoUI() {
  tryButton.disabled = false;
  stopButton.disabled = true;
  container.innerHTML = "";
}
tryButton.addEventListener("click", startVideo);


document.addEventListener("DOMContentLoaded", function(event) { 

startVideo();

  
});