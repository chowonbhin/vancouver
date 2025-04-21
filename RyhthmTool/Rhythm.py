from typing import List, Tuple
from pydub import AudioSegment
from pydub.effects import normalize
from pydub.silence import detect_silence, detect_nonsilent
import librosa
import numpy as np
from scipy.signal import butter, sosfiltfilt
import matplotlib.pyplot as plt


def detect_onset_trim(audio: AudioSegment, sr: int, min_silence_len: int = 50, silence_thresh: int = -40) -> Tuple[int, int]:
    """
    Detects onset and offset in an audio segment using nonsilent regions.

    :param audio: Pydub AudioSegment
    :param sr: Sample rate (Hz)
    :param min_silence_len: Minimum silence length (ms)
    :param silence_thresh: Silence threshold (dB)
    :return: Onset and offset positions in samples
    """
    nonsilent_ranges = detect_nonsilent(audio, min_silence_len=min_silence_len, silence_thresh=silence_thresh)
    if not nonsilent_ranges:
        return None, None
    onset_ms, offset_ms = nonsilent_ranges[0][0], nonsilent_ranges[-1][1]
    onset_sample = int(onset_ms * sr / 1000)
    offset_sample = int(offset_ms * sr / 1000)
    return onset_sample, offset_sample


def extract_audio_chunks(audio: AudioSegment, min_silence_len: int = 1000, silence_thresh: int = -20) -> List[Tuple[float, float]]:
    """
    Extracts audio chunks based on silent segments.

    :param audio: Pydub AudioSegment
    :param min_silence_len: Minimum silence length (ms)
    :param silence_thresh: Silence threshold (dB)
    :return: List of (start, end) times for each chunk in seconds
    """
    silent_parts = detect_silence(audio, min_silence_len=min_silence_len, silence_thresh=silence_thresh)
    chunks = []
    previous_end = 0
    for start, end in silent_parts:
        if previous_end < start:
            chunks.append((previous_end / 1000, start / 1000))
        previous_end = end
    if previous_end < len(audio):
        chunks.append((previous_end / 1000, len(audio) / 1000))
    return chunks


def GetAudioSegment(file: str) -> AudioSegment:
    """
    Loads an audio file as a mono normalized AudioSegment.

    :param file: Path to audio file
    :return: Normalized mono AudioSegment
    """
    audio = AudioSegment.from_file(file).set_channels(1)
    return normalize(audio)


def GetAudioArray(audio: AudioSegment) -> np.ndarray:
    """
    Converts AudioSegment to a NumPy array.

    :param audio: AudioSegment
    :return: NumPy array of audio samples
    """
    dtype_map = {1: np.int8, 2: np.int16, 4: np.int32}
    if audio.sample_width not in dtype_map:
        raise ValueError(f"Unsupported sample width: {audio.sample_width}")
    samples = np.frombuffer(audio._data, dtype=dtype_map[audio.sample_width])
    return samples.astype(np.float64)


def GetAudio(audioArray: np.ndarray, frame_rate: int, sample_width: int, channels: int) -> AudioSegment:
    """
    Converts a NumPy array back to AudioSegment.

    :param audioArray: NumPy array of audio data
    :param frame_rate: Sample rate (Hz)
    :param sample_width: Sample width (bytes)
    :param channels: Number of channels
    :return: AudioSegment object
    """
    dtype_map = {1: np.int8, 2: np.int16, 4: np.int32}
    if sample_width not in dtype_map:
        raise ValueError(f"Unsupported sample width: {sample_width}")
    audioArray = audioArray.astype(dtype_map[sample_width])
    audio = AudioSegment(audioArray.tobytes(), frame_rate=frame_rate, sample_width=sample_width, channels=channels)
    return normalize(audio).set_channels(1)


def PrintAudioInfo(audio: AudioSegment) -> None:
    """
    Prints key audio information.

    :param audio: AudioSegment
    """
    print("Channels:", audio.channels)
    print("Sample rate:", audio.frame_rate)
    print("Duration:", audio.duration_seconds)
    print("Bit depth:", audio.sample_width, "bytes")
    print("len samples:", len(np.array(audio.get_array_of_samples())))
    display(audio)


def STFT(audio_samples: np.ndarray, sr: int, n_fft: int = 512, hop_length: int = 256) -> np.ndarray:
    """
    Computes the Short-Time Fourier Transform (STFT) of audio samples.

    :param audio_samples: Audio sample array
    :param sr: Sample rate
    :param n_fft: FFT window size
    :param hop_length: Hop length for STFT
    :return: STFT complex matrix
    """
    return librosa.stft(audio_samples, n_fft=n_fft, hop_length=hop_length)


def Butter_filter(data: np.ndarray, sr: int, lowcut: int, highcut: int, type: str, order: int = 5) -> np.ndarray:
    """
    Applies a Butterworth bandpass or bandstop filter.

    :param data: Input signal
    :param sr: Sample rate
    :param lowcut: Low frequency cut-off
    :param highcut: High frequency cut-off
    :param type: Filter type ('bandpass', 'bandstop')
    :param order: Filter order
    :return: Filtered signal
    """
    nyq = 0.5 * sr
    low = lowcut / nyq
    high = highcut / nyq
    sos = butter(order, [low, high], btype=type, analog=False, output='sos')
    return sosfiltfilt(sos, data)


def GetRms(signal: np.ndarray, sr: int, frame_length: int, hop_length: int) -> Tuple[float, np.ndarray]:
    """
    Computes root mean square (RMS) energy of the signal.

    :param signal: Audio signal
    :param sr: Sample rate
    :param frame_length: Frame size for RMS
    :param hop_length: Hop length for RMS
    :return: Tuple of (global RMS, per-sample RMS)
    """
    rms = librosa.feature.rms(y=signal, frame_length=frame_length, hop_length=hop_length)[0]
    global_rms = np.mean(rms)
    full_rms = np.zeros_like(signal)
    for i, val in enumerate(rms):
        start = i * hop_length
        end = min(len(signal), start + frame_length)
        full_rms[start:end] = val
    return global_rms, full_rms
