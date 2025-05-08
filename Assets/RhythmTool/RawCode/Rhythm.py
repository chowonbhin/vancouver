from typing import List, Tuple
from pydub import AudioSegment
from pydub.effects import normalize
from pydub.silence import detect_silence, detect_nonsilent
import librosa
import numpy as np
from scipy.signal import butter, sosfiltfilt
import matplotlib.pyplot as plt

def GetAudioSegment(file: str) -> AudioSegment:
    audio = AudioSegment.from_file(file).set_channels(1)
    return normalize(audio)
def GetAudioArray(audio: AudioSegment) -> np.ndarray:
    dtype_map = {1: np.int8, 2: np.int16, 4: np.int32}
    if audio.sample_width not in dtype_map:
        raise ValueError(f"Unsupported sample width: {audio.sample_width}")
    samples = np.frombuffer(audio._data, dtype=dtype_map[audio.sample_width])
    return samples.astype(np.float64)
def GetAudio(audioArray: np.ndarray, frame_rate: int, sample_width: int, channels: int) -> AudioSegment:
    dtype_map = {1: np.int8, 2: np.int16, 4: np.int32}
    if sample_width not in dtype_map:
        raise ValueError(f"Unsupported sample width: {sample_width}")
    audioArray = audioArray.astype(dtype_map[sample_width])
    audio = AudioSegment(audioArray.tobytes(), frame_rate=frame_rate, sample_width=sample_width, channels=channels)
    return normalize(audio).set_channels(1)
def AverageSmoothing(array,frame_length,n_iterations):
    averaged_array= array.copy()
    for _ in range(n_iterations):
        padded_array = np.pad(averaged_array, (frame_length // 2, frame_length // 2), mode='edge')
        averaged_array = np.convolve(padded_array, np.ones(frame_length)/frame_length, mode='Valid')[:-1]
    return averaged_array
def Detect(array,frame_length,frame_rate,hop_length,uppderPoint):
    is_signal = np.ones_like(array, dtype=bool)
    for start_idx in range(0, len(array) - frame_length + 1, hop_length):
        end_idx = start_idx + frame_length
        window = array[start_idx:end_idx]
        range_percentile = np.arange(1, 101)
        percentiles = np.percentile(abs(window), range_percentile)
        noiseThreshold = percentiles[uppderPoint]
        is_signal[start_idx:end_idx] &= (abs(window) > noiseThreshold)
        
    if (len(array) - frame_length) % hop_length != 0:
        start_idx = len(array) - frame_length
        end_idx = len(array)
        window = array[start_idx:end_idx]
        percentiles = np.percentile(window, range_percentile)
        noiseThreshold = percentiles[uppderPoint]
        is_signal[start_idx:end_idx] &= (window > noiseThreshold)

    return is_signal
def PrintAudioInfo(audio: AudioSegment) -> None:
    print("Channels:", audio.channels)
    print("Sample rate:", audio.frame_rate)
    print("Duration:", audio.duration_seconds)
    print("Bit depth:", audio.sample_width, "bytes")
    print("len samples:", len(np.array(audio.get_array_of_samples())))
    display(audio)