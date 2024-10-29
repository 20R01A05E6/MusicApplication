let currentSong = null; // Keeps track of the currently playing song

function togglePlay(songId) {
    const audioElement = document.getElementById(`audio-${songId}`);
    const playIcon = document.getElementById(`play-icon-${songId}`);
    const pauseIcon = document.getElementById(`pause-icon-${songId}`);

    // If there's a currently playing song and it's different from the clicked one, stop the current one
    if (currentSong && currentSong !== songId) {
        const currentAudio = document.getElementById(`audio-${currentSong}`);
        const currentPlayIcon = document.getElementById(`play-icon-${currentSong}`);
        const currentPauseIcon = document.getElementById(`pause-icon-${currentSong}`);

        currentAudio.pause();
        currentPlayIcon.style.display = 'none'; // Show play icon
        currentPauseIcon.style.display = 'block'; // Hide pause icon
    }

    // Toggle play/pause for the clicked song
    if (audioElement.paused) {
        audioElement.play();
        playIcon.style.display = 'block'; // Hide play icon
        pauseIcon.style.display = 'none'; // Show pause icon
        currentSong = songId; // Set the current song
    } else {
        audioElement.pause();
        playIcon.style.display = 'none'; // Show play icon
        pauseIcon.style.display = 'block'; // Hide pause icon
        currentSong = null; // Clear the current song
    }
}