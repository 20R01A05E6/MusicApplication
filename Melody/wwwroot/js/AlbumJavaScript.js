let currentSong = null; // Store currently playing song
    let currentIndex = -1; // Track the index of the current song in the array
    let songElements = []; // Store all audio elements

    // Initialize the song elements once the DOM is fully loaded
    document.addEventListener('DOMContentLoaded', function () {
        songElements = Array.from(document.querySelectorAll('audio'));
    });

    function togglePlay(songId, filePath) {
        const audioElement = document.getElementById(`audio-${songId}`);
        const playIcon = document.getElementById(`play-icon-${songId}`);
        const pauseIcon = document.getElementById(`pause-icon-${songId}`);
        const playAllCheckbox = document.getElementById('play-all-toggle');

        if (currentSong && currentSong !== songId) {
            // Pause the current song and revert the play/pause icon
            const currentAudio = document.getElementById(`audio-${currentSong}`);
            currentAudio.pause();
            document.getElementById(`play-icon-${currentSong}`).style.display = 'none'; // Hide play icon
            document.getElementById(`pause-icon-${currentSong}`).style.display = 'block'; // Show pause icon
            document.getElementById(`play-toggle-${currentSong}`).checked = false; // Reset checkbox state
        }

        // If Play All is active, deactivate it when an individual song is played
        if (playAllCheckbox.checked) {
            playAllCheckbox.checked = false; // Revert "Play All" button to default state
            stopAllSongs(); // Stop all songs if any are playing under Play All
        }

        if (audioElement.paused) {
            // Play the clicked song
            audioElement.play();
            playIcon.style.display = 'block'; // Show play icon when playing
            pauseIcon.style.display = 'none'; // Hide pause icon
            currentSong = songId; // Update current song ID
        } else {
            // Pause the song
            audioElement.pause();
            playIcon.style.display = 'none'; // Hide play icon
            pauseIcon.style.display = 'block'; // Show pause icon when paused
            currentSong = null; // Reset current song ID
        }
    }

    function togglePlayAll() {
        const playAllCheckbox = document.getElementById('play-all-toggle');
        const isPlaying = playAllCheckbox.checked;

        if (isPlaying) {
            // Stop any individual song playing
            if (currentSong) {
                const currentAudio = document.getElementById(`audio-${currentSong}`);
                currentAudio.pause();
                document.getElementById(`play-icon-${currentSong}`).style.display = 'none'; // Hide play icon
                document.getElementById(`pause-icon-${currentSong}`).style.display = 'block'; // Show pause icon
                document.getElementById(`play-toggle-${currentSong}`).checked = false; // Reset checkbox state
                currentSong = null; // Reset current song ID
            }
            currentIndex = 0; // Start from the first song
            playNextSong();
        } else {
            // Stop all songs if the play-all button is unchecked
            stopAllSongs();
            currentSong = null; // Reset current song ID
            currentIndex = -1; // Reset index
        }
    }

    function stopAllSongs() {
        songElements.forEach((audio, index) => {
            audio.pause();
            document.getElementById(`play-icon-${index + 1}`).style.display = 'none'; // Hide play icons
            document.getElementById(`pause-icon-${index + 1}`).style.display = 'block'; // Show pause icons
        });
    }

    function playNextSong() {
        if (currentIndex >= 0 && currentIndex < songElements.length) {
            const currentAudio = songElements[currentIndex];
            const playIcon = document.getElementById(`play-icon-${currentIndex + 1}`);
            const pauseIcon = document.getElementById(`pause-icon-${currentIndex + 1}`);

            // Check if playIcon and pauseIcon exist to avoid null errors
            if (playIcon && pauseIcon) {
                // Play the current song
                currentAudio.play();
                playIcon.style.display = 'block'; // Show play icon
                pauseIcon.style.display = 'none'; // Hide pause icon

                currentAudio.onended = function () {
                    playIcon.style.display = 'none'; // Hide play icon
                    pauseIcon.style.display = 'block'; // Show pause icon
                    currentIndex++; // Increment index
                    if (currentIndex < songElements.length) {
                        playNextSong(); // Play the next song
                    } else {
                        document.getElementById('play-all-toggle').checked = false; // Uncheck play-all checkbox
                        currentIndex = -1; // Reset index
                    }
                };
            } else {
                console.error(`Element with id play-icon-${currentIndex + 1} or pause-icon-${currentIndex + 1} not found.`);
            }
        }
    }

    // Add event listener for Play All toggle
    document.getElementById('play-all-toggle').addEventListener('change', togglePlayAll);